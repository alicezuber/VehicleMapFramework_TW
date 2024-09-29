﻿using Verse;

namespace VehicleInteriors
{
    public class Verb_ShootOneUseOnVehicle : Verb_ShootOnVehicle
    {
        protected override bool TryCastShot()
        {
            if (base.TryCastShot())
            {
                if (this.burstShotsLeft <= 1)
                {
                    this.SelfConsume();
                }
                return true;
            }
            if (this.burstShotsLeft < this.verbProps.burstShotCount)
            {
                this.SelfConsume();
            }
            return false;
        }

        public override void Notify_EquipmentLost()
        {
            base.Notify_EquipmentLost();
            if (this.state == VerbState.Bursting && this.burstShotsLeft < this.verbProps.burstShotCount)
            {
                this.SelfConsume();
            }
        }

        private void SelfConsume()
        {
            if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
            {
                base.EquipmentSource.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
