using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class CombatClass
    {
        private const float BASE_MELEERANGE_OFFSET = 1.33f;
        private const float MIN_ATTACK_DISTANCE = 4f;
        private static ICombatClass _instanceFromOtherAssembly;
        private static Assembly _assembly;
        private static object _obj;
        private static Thread _worker;
        private static string _pathToCombatClassFile = "";
        private static string _threadName = "";
        private static BigInteger _forceBigInteger = 1000000000; // Force loading System.Numerics assembly when not running in VS.
        private static Thread _combatClassLoader;
        private static readonly object CombatClassLocker = new object();

        private static bool isMeleeClass
        {
            get { return GetRange <= 5.0f; }
        }

        public static float GetRange
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                        return _instanceFromOtherAssembly.Range < 1.5f ? 1.5f : _instanceFromOtherAssembly.Range;
                    return 1.5f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CombatClass > GetRange: " + exception);
                    return 1.5f;
                }
            }
        }

        public static float GetAggroRange
        {
            get
            {
                try
                {
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (_instanceFromOtherAssembly.AggroRange < _instanceFromOtherAssembly.Range)
                            return _instanceFromOtherAssembly.Range;
                        return _instanceFromOtherAssembly.AggroRange < 1.5f ? 1.5f : _instanceFromOtherAssembly.AggroRange;
                    }
                    return 1.5f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CombatClass > GetAggroRange: " + exception);
                    return 1.5f;
                }
            }
        }

        public static bool IsAliveCombatClass
        {
            get
            {
                try
                {
                    return _worker != null && _worker.IsAlive;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("IsAliveCombatClass: " + exception);
                    return false;
                }
            }
        }

        public static Spell GetLightHealingSpell
        {
            get
            {
                if (_instanceFromOtherAssembly != null && _instanceFromOtherAssembly.LightHealingSpell != null && _instanceFromOtherAssembly.LightHealingSpell.Id > 0)
                    return _instanceFromOtherAssembly.LightHealingSpell;
                return new Spell(0);
            }
        }

        private static float CombatDistance(WoWUnit unit, bool MeleeSpell = true)
        {
            float reach = unit.GetCombatReach + ObjectManager.ObjectManager.Me.GetCombatReach;
            if (MeleeSpell)
                reach += BASE_MELEERANGE_OFFSET;
            if (MeleeSpell && reach < MIN_ATTACK_DISTANCE)
                reach = MIN_ATTACK_DISTANCE;
            return reach;
        }

        public static bool InMeleeRange(WoWUnit unit)
        {
            return unit.GetDistance < CombatDistance(unit, true) - 0.2f;
        }

        public static bool InRange(WoWUnit unit)
        {
            if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
                return HealerClass.InRange(unit);
            return unit.GetDistance < CombatDistance(unit) + (isMeleeClass ? -1f : GetRange);
        }

        public static bool InAggroRange(WoWUnit unit)
        {
            if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
                return HealerClass.InRange(unit);
            float unitCombatReach = unit.GetCombatReach;
            float reach = unitCombatReach + GetAggroRange;
            return unit.GetDistance < reach;
        }

        public static bool InSpellRange(WoWUnit unit, float minRange, float maxRange)
        {
            try
            {
                if (!IsAliveCombatClass && HealerClass.IsAliveHealerClass)
                    return HealerClass.InCustomRange(unit, minRange, maxRange);
                float distance = unit.GetDistance;
                float reach;
                if (maxRange <= 5.0f) // Melee spell
                    reach = CombatDistance(unit, true);
                else
                    reach = CombatDistance(unit, false);
                return distance <= reach + maxRange && (minRange == 0f || distance >= reach + minRange);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CombatClass > InCustomRange: " + exception);
                return false;
            }
        }

        public static bool AboveMinRange(WoWUnit unit)
        {
            // Can't find any meaning to this without a spell
            // so let's assume 2f is too close even for a melee
            return unit.GetDistance < 2.0f;
        }

        public static void LoadCombatClass()
        {
            lock (CombatClassLocker)
            {
                if (_worker != null && _worker.IsAlive || _combatClassLoader != null && _combatClassLoader.IsAlive)
                    return;
                _combatClassLoader = new Thread(LoadCombatClassThread) {Name = "Load Combat Class"};
                _combatClassLoader.Start();
            }
        }

        public static void LoadCombatClassThread()
        {
            try
            {
                string __pathToCombatClassFile;
                if (nManagerSetting.CurrentSetting.CombatClass != "")
                {
                    if (nManagerSetting.CurrentSetting.CombatClass == "OfficialTnbClassSelector")
                    {
                        __pathToCombatClassFile = Application.StartupPath + "\\CombatClasses\\OfficialTnbClassSelector\\Tnb_" + ObjectManager.ObjectManager.Me.WowClass + "Rotations.dll";
                    }
                    else
                        __pathToCombatClassFile = Application.StartupPath + "\\CombatClasses\\" + nManagerSetting.CurrentSetting.CombatClass;
                    string fileExt = __pathToCombatClassFile.Substring(__pathToCombatClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCombatClass(__pathToCombatClassFile, false, false, false);
                    else
                        LoadCombatClass(__pathToCombatClassFile);
                }
                else
                    Logging.Write("No custom class selected");
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCombatClass(): " + exception);
            }
        }

        public static void LoadCombatClass(string pathToCombatClassFile, bool settingOnly = false,
            bool resetSettings = false,
            bool CSharpFile = true)
        {
            try
            {
                _pathToCombatClassFile = pathToCombatClassFile;
                if (_instanceFromOtherAssembly != null)
                {
                    _instanceFromOtherAssembly.Dispose();
                }

                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;

                if (CSharpFile)
                {
                    CodeDomProvider cc = new CSharpCodeProvider();
                    var cp = new CompilerParameters();
                    IEnumerable<string> assemblies = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where(
                            a =>
                                !a.IsDynamic &&
                                !a.CodeBase.Contains((Process.GetCurrentProcess().ProcessName + ".exe")))
                        .Select(a => a.Location);
                    cp.ReferencedAssemblies.AddRange(assemblies.ToArray());
                    StreamReader sr = File.OpenText(pathToCombatClassFile);
                    string toCompile = sr.ReadToEnd();
                    CompilerResults cr = cc.CompileAssemblyFromSource(cp, toCompile);
                    if (cr.Errors.HasErrors)
                    {
                        String text = cr.Errors.Cast<CompilerError>().Aggregate("Compilator Error :\n",
                            (current, err) => current + (err + "\n"));
                        Logging.WriteError(text);
                        MessageBox.Show(text);
                        return;
                    }

                    _assembly = cr.CompiledAssembly;
                    _obj = _assembly.CreateInstance("Main", true);
                    _threadName = "CombatClass CS";
                }
                else
                {
                    _assembly = Assembly.LoadFrom(_pathToCombatClassFile);
                    _obj = _assembly.CreateInstance("Main", false);
                    _threadName = "CombatClass DLL";
                }
                if (_obj != null && _assembly != null)
                {
                    _instanceFromOtherAssembly = _obj as ICombatClass;
                    if (_instanceFromOtherAssembly != null)
                    {
                        if (settingOnly)
                        {
                            if (resetSettings)
                                _instanceFromOtherAssembly.ResetConfiguration();
                            else
                                _instanceFromOtherAssembly.ShowConfiguration();
                            _instanceFromOtherAssembly.Dispose();
                            return;
                        }

                        _worker = new Thread(_instanceFromOtherAssembly.Initialize)
                        {IsBackground = true, Name = _threadName};
                        _worker.Start();
                    }
                    else
                        Logging.WriteError("Custom Class Loading error.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadCombatClass(string _pathToCombatClassFile): " + exception);
            }
        }

        public static void DisposeCombatClass()
        {
            try
            {
                lock (CombatClassLocker)
                {
                    if (_instanceFromOtherAssembly != null)
                    {
                        _instanceFromOtherAssembly.Dispose();
                    }
                    if (_worker != null)
                    {
                        if (_worker.IsAlive)
                        {
                            _worker.Abort();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisposeCombatClass(): " + exception);
            }
            finally
            {
                _instanceFromOtherAssembly = null;
                _assembly = null;
                _obj = null;
            }
        }

        public static void ResetCombatClass()
        {
            try
            {
                if (IsAliveCombatClass)
                {
                    DisposeCombatClass();
                    Thread.Sleep(1000);
                    string fileExt = _pathToCombatClassFile.Substring(_pathToCombatClassFile.Length - 3);
                    if (fileExt == "dll")
                        LoadCombatClass(_pathToCombatClassFile, false, false, false);
                    else
                        LoadCombatClass(_pathToCombatClassFile);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetCombatClass(): " + exception);
            }
        }

        public static void ShowConfigurationCombatClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCombatClass(filePath, true, false, false);
                else
                    LoadCombatClass(filePath, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception);
            }
        }

        public static void ResetConfigurationCombatClass(string filePath)
        {
            try
            {
                string fileExt = filePath.Substring(filePath.Length - 3);
                if (fileExt == "dll")
                    LoadCombatClass(filePath, true, true, false);
                else
                    LoadCombatClass(filePath, true, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowConfigurationCombatClass(): " + exception);
            }
        }
    }


    public interface ICombatClass
    {
        #region Properties

        float Range { get; }
        float AggroRange { get; }
        Spell LightHealingSpell { get; }

        #endregion Properties

        #region Methods

        void Initialize();

        void Dispose();

        void ShowConfiguration();

        void ResetConfiguration();

        #endregion Methods
    }
}