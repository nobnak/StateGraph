﻿using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace StateGraphSystem {

    public class Test : MonoBehaviour {
        public const string FORMAT_TRIGGER = "Triggered ({0})-->({1})";

        enum VitalState { Dead = 0, Alive }
        enum TransitionState { None = 0, Spawning, Dying }

     	void Start () {
            var smVital = new StateMachine<VitalState> (VitalState.Dead);
            var smTransition = new StateMachine<TransitionState> (TransitionState.None);

            smTransition.Tr (TransitionState.None, TransitionState.Spawning).Cond (
                () => smVital.Current == VitalState.Dead);
            smTransition.Tr (TransitionState.None, TransitionState.Dying).Cond (
                () => smVital.Current == VitalState.Alive);
            smTransition.Tr (TransitionState.Spawning, TransitionState.None).Cond (
                () => smVital.Current == VitalState.Dead);
            smTransition.Tr (TransitionState.Dying, TransitionState.None).Cond (
                () => smVital.Current == VitalState.Alive);

            smVital.Tr (VitalState.Dead, VitalState.Alive).Cond (
                () => smTransition.Current == TransitionState.Spawning && smTransition.Next (TransitionState.None));
            smVital.Tr (VitalState.Alive, VitalState.Dead).Cond (
                () => smTransition.Current == TransitionState.Dying && smTransition.Next (TransitionState.None));

            Assert.IsTrue (smTransition.Next (TransitionState.Spawning));
            Assert.AreEqual (smTransition.Current, TransitionState.Spawning);
            Assert.AreEqual (smVital.Current, VitalState.Dead);

            Assert.IsTrue (smVital.Next (VitalState.Alive));
            Assert.AreEqual (smVital.Current, VitalState.Alive);
            Assert.AreEqual (smTransition.Current, TransitionState.None);

            Assert.IsTrue (smTransition.Next (TransitionState.Dying));
            Assert.AreEqual (smTransition.Current, TransitionState.Dying);
            Assert.AreEqual (smVital.Current, VitalState.Alive);

            Assert.IsTrue (smVital.Next (VitalState.Dead));
            Assert.AreEqual (smVital.Current, VitalState.Dead);
            Assert.AreEqual (smTransition.Current, TransitionState.None);
        }            
    }
}