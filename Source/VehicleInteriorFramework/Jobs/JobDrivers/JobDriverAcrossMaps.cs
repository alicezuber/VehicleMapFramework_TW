﻿using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VehicleInteriors
{
    public abstract class JobDriverAcrossMaps : JobDriver
    {
        public bool ShouldEnterTargetAMap => this.exitSpot1.Map != null || this.enterSpot1.Map != null;

        public bool ShouldEnterTargetBMap => this.exitSpot2.Map != null || this.enterSpot2.Map != null;

        public Map DestMap
        {
            get
            {
                if (this.destMap != null) return this.destMap;
                if (this.enterSpot2.Map != null) return this.enterSpot2.Map;
                if (this.exitSpot2.Map != null) return this.exitSpot2.Map.BaseMap();
                if (this.enterSpot1.Map != null) return this.enterSpot1.Map;
                if (this.exitSpot1.Map != null) return this.exitSpot1.Map.BaseMap();
                return base.Map;
            }
        }

        public Map TargetAMap
        {
            get
            {
                if (this.targetAMap != null) return this.targetAMap;
                if (this.enterSpot1.Map != null) return this.enterSpot1.Map;
                if (this.exitSpot1.Map != null) return this.exitSpot1.Map.BaseMap();
                return base.Map;
            }
        }

        public override Vector3 ForcedBodyOffset
        {
            get
            {
                return this.drawOffset;
            }
        }

        public void SetSpots(TargetInfo? exitSpot1 = null, TargetInfo? enterSpot1 = null, TargetInfo? exitSpot2 = null, TargetInfo? enterSpot2 = null)
        {
            this.exitSpot1 = exitSpot1 ?? TargetInfo.Invalid;
            this.enterSpot1 = enterSpot1 ?? TargetInfo.Invalid;
            this.exitSpot2 = exitSpot2 ?? TargetInfo.Invalid;
            this.enterSpot2 = enterSpot2 ?? TargetInfo.Invalid;
            this.targetAMap = this.TargetAMap;
            this.destMap = this.DestMap;

            if (this.exitSpot1.IsValid && this.exitSpot1.Map == null ||
                this.enterSpot1.IsValid && this.enterSpot1.Map == null ||
                this.exitSpot2.IsValid && this.exitSpot2.Map == null ||
                this.enterSpot2.IsValid && this.enterSpot2.Map == null)
            {
                Log.Error("[VehicleMapFramework] SetSpots with null map.");
            }
        }

        public IEnumerable<Toil> GotoTargetMap(TargetIndex ind)
        {
            if (ind == TargetIndex.A)
            {
                return ToilsAcrossMaps.GotoTargetMap(this, this.exitSpot1, this.enterSpot1);
            }
            if (ind == TargetIndex.B)
            {
                return ToilsAcrossMaps.GotoTargetMap(this, this.exitSpot2, this.enterSpot2);
            }
            Log.Error("[VehicleMapFramework] GotoTargetMap() does not support TargetIndex.C.");
            return null;
        }

        public override void ExposeData()
        {
            Scribe_TargetInfo.Look(ref this.exitSpot1, "exitSpot1");
            Scribe_TargetInfo.Look(ref this.enterSpot1, "enterSpot1");
            Scribe_TargetInfo.Look(ref this.exitSpot2, "exitSpot2");
            Scribe_TargetInfo.Look(ref this.enterSpot2, "enterSpot2");
            Scribe_Values.Look(ref this.drawOffset, "drawOffset");
            Scribe_References.Look(ref this.targetAMap, "targetAMap");
            Scribe_References.Look(ref this.destMap, "destMap");
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                base.ExposeData();
            });
        }

        private TargetInfo exitSpot1 = TargetInfo.Invalid;

        private TargetInfo enterSpot1 = TargetInfo.Invalid;

        private TargetInfo exitSpot2 = TargetInfo.Invalid;

        private TargetInfo enterSpot2 = TargetInfo.Invalid;

        public Vector3 drawOffset;

        private Map targetAMap;

        private Map destMap;
    }
}