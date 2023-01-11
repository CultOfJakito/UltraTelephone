using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

public class ZedPatches
{
    // Disable weapon switching so the mod isn't useless
    [HarmonyPatch(typeof(PlayerInput), "Slot1", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot1(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "Slot2", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot2(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "Slot3", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot3(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "Slot4", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot4(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "Slot5", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableSlot5(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "ChangeVariation", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableChangeVariaion(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "LastWeapon", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableLastWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "NextWeapon", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableNextWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "PrevWeapon", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisablePrevWeapon(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
    [HarmonyPatch(typeof(PlayerInput), "WheelLook", MethodType.Getter)]
    [HarmonyPrefix]
    public static bool DisableWheelLook(ref InputActionState __result)
    {
        InputAction action = new InputAction();
        InputActionState state = new InputActionState(action);
        __result = state;
        return false;
    }
}
