using SlimDX;
using SlimDX.DXGI;
using SlimDX.Windows;
using Device = SlimDX.Direct3D11.Device;
using Format = SlimDX.DXGI.Format;
using SwapChain = SlimDX.DXGI.SwapChain;
using SwapEffect = SlimDX.DXGI.SwapEffect;
using Usage = SlimDX.DXGI.Usage;

namespace nManager.Wow.MemoryClass
{
    internal class D3D
    {
        public static bool IsD3D11(int processId)
        {
            return D3D9Adresse(processId) == 0;
        }

        private static uint d3d11Adresse;

        public static uint D3D11Adresse()
        {
            if (d3d11Adresse <= 0)
            {
                const int VMT_PRESENT = 8;
                using (var rf = new RenderForm())
                {
                    var desc = new SwapChainDescription
                        {
                            BufferCount = 1,
                            Flags = SwapChainFlags.None,
                            IsWindowed = true,
                            ModeDescription =
                                new ModeDescription(100, 100, new Rational(60, 1),
                                                    Format.R8G8B8A8_UNorm),
                            OutputHandle = rf.Handle,
                            SampleDescription = new SampleDescription(1, 0),
                            SwapEffect = SwapEffect.Discard,
                            Usage = Usage.RenderTargetOutput
                        };

                    Device tmpDevice;
                    SwapChain sc;
                    var res = Device.CreateWithSwapChain(SlimDX.Direct3D11.DriverType.Hardware,
                                                         SlimDX.Direct3D11.DeviceCreationFlags.None, desc, out tmpDevice,
                                                         out sc);
                    if (res.IsSuccess)
                    {
                        using (tmpDevice)
                        {
                            using (sc)
                            {
                                var memory = new Magic.BlackMagic(System.Diagnostics.Process.GetCurrentProcess().Id);
                                d3d11Adresse = memory.ReadUInt(memory.ReadUInt((uint) sc.ComPointer) + 4*VMT_PRESENT);
                            }
                        }
                    }
                }
            }
            return d3d11Adresse;
        }

        private static uint d3d9Adresse;

        public static uint D3D9Adresse(int processId)
        {
            var memory = new Magic.BlackMagic(processId);
            uint pDevice =
                memory.ReadUInt((uint) memory.GetModule("Wow.exe").BaseAddress +
                                (uint) Patchables.Addresses.Hooking.DX_DEVICE);
            uint pEnd = memory.ReadUInt(pDevice + (uint) Patchables.Addresses.Hooking.DX_DEVICE_IDX);
            uint pScene = memory.ReadUInt(pEnd);
            d3d9Adresse = memory.ReadUInt(pScene + (uint) Patchables.Addresses.Hooking.ENDSCENE_IDX);

            return d3d9Adresse;
        }

        public static byte[] OriginalBytes { get; set; }

        //public static string[] OriginalOpcode = new[] { "mov edi,edi", "push ebp", "mov ebp,esp" };
    }
}