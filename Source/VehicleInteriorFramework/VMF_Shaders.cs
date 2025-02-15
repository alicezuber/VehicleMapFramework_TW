﻿using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VehicleInteriors
{
    [StaticConstructorOnStartup]
    public static class VMF_Shaders
    {
        public static Shader LoadShader(string shaderPath)
        {
            var lookup = AccessTools.FieldRefAccess<Dictionary<string, Shader>>(typeof(ShaderDatabase), "lookup")() ?? new Dictionary<string, Shader>();
            if (!lookup.ContainsKey(shaderPath))
            {
                lookup[shaderPath] = VehicleInteriors.Bundle.LoadAsset<Shader>($"Assets/Data/VehicleInteriors/{shaderPath}.shader");
            }
            Shader shader = lookup[shaderPath];

            if (shader == null)
            {
                Log.Warning("Could not load shader " + shaderPath);
                return ShaderDatabase.DefaultShader;
            }
            return shader;
        }

        public static Shader terrainHardWithZ = VMF_Shaders.LoadShader("terrainHardWithZ");
    }
}