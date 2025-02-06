using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace STRAFTSHAT.Features
{
    public class WeaponMods
    {
        private static void InfiniteAmmo(Weapon weapon)
        {
            if (!weapon)
                return;

            weapon.ammoCharge = 999;
            weapon.currentAmmo = 999;
        }

        private static void RapidFire(Weapon weapon)
        {
            if (!weapon)
                return;

            weapon.onePressShoot = false;
            weapon.timeBetweenBullets = 0;
            weapon.timeBetweenFire = 0;
        }

        private static void InstaKill(Weapon weapon)
        {
            if (!weapon)
                return;

            weapon.damage = 999;
        }

        private static void NoSpread(Weapon weapon)
        {
            if (!weapon)
                return;

            weapon.minSpread = 0;
            weapon.maxSpread = 0;
        }

        private static void WeaponSpeed(Weapon weapon)
        {
            if (!weapon)
                return;

            weapon.movementFactor = 3;
            weapon.fireSlowDownFactor = 0;
            weapon.fireSlowDownDuration = 0;
        }

        public static void Update()
        {
            Weapon leftWeapon = Cheat.Instance.Cache.LocalWeaponLeft;
            Weapon rightWeapon = Cheat.Instance.Cache.LocalWeaponRight;

            if (Config.Instance.InfiniteAmmo)
            {
                InfiniteAmmo(leftWeapon);
                InfiniteAmmo(rightWeapon);
            }

            if (Config.Instance.RapidFire)
            {
                RapidFire(leftWeapon);
                RapidFire(rightWeapon);
            }

            if (Config.Instance.InstaKill)
            {
                InstaKill(leftWeapon);
                InstaKill(rightWeapon);
            }

            if (Config.Instance.NoSpread)
            {
                NoSpread(leftWeapon);
                NoSpread(rightWeapon);
            }

            if (Config.Instance.WeaponSpeed)
            {
                WeaponSpeed(leftWeapon);
                WeaponSpeed(rightWeapon);
            }

        }
    }
}
