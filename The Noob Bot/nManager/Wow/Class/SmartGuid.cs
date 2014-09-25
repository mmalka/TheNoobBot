/*
 * Copyright (C) 2012-2014 Arctium Emulation <http://arctium.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Enums;

namespace nManager.Wow.Class
{
    /// <summary>
    /// SmartGuid Class
    /// </summary>
    public class SmartGuid
    {
        public ulong Low { get; set; }
        public ulong High { get; set; }

        public SmartGuid() { }

        public SmartGuid(WoWObject obj)
        {
            WoWPlayer player = ObjectManager.ObjectManager.GetObjectWoWPlayer(obj.Guid);
            if (player != null)
            {
                Type         = GuidType.Player;
                MapId        = (ushort)Usefuls.GetMapId();
                CreationBits = player.Guid;
            }
        }

        public GuidType Type
        {
            get { return (GuidType)(High >> 58); }
            set { High |= (ulong)value << 58; }
        }

        public GuidSubType SubType
        {
            get { return (GuidSubType)(Low >> 56); }
            set { Low |= (ulong)value << 56; }
        }

        public ushort RealmId
        {
            get { return (ushort)((High >> 42) & 0x1FFF); }
            set { High |= (ulong)value << 42; }
        }

        public ushort ServerId
        {
            get { return (ushort)((Low >> 40) & 0x1FFF); }
            set { Low |= (ulong)value << 40; }
        }

        public ushort MapId
        {
            get { return (ushort)((High >> 29) & 0x1FFF); }
            set { High |= (ulong)value << 29; }
        }

        public uint Id
        {
            get { return (uint)(High & 0xFFFFFF) >> 6; }
            set { High |= (ulong)value << 6; }
        }

        public ulong CreationBits
        {
            get { return Low & 0xFFFFFFFFFF; }
            set { Low |= value; }
        }
    }
}
