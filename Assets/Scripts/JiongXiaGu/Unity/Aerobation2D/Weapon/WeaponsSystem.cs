using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Aerobation2D
{

    public class WeaponsSystem
    {
        private IList<IEnumerable<IWeapon>> weapons;

        public void FireAll()
        {
            foreach (var item in weapons)
            {
                if (item != null)
                {
                    foreach (var weapon in item)
                    {
                        weapon.Fire();
                    }
                }
            }
        }

        public bool Fire(int number)
        {
            if (weapons.Count > number)
            {
                var weapon = weapons[number];
                if (weapon != null)
                {

                }
            }
            return false;
            throw new NotImplementedException();
        }
    }
}
