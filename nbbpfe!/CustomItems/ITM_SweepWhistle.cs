﻿using MTM101BaldAPI.Reflection;
using nbppfe.Extensions;
using nbppfe.PrefabSystem;
using UnityEngine;

namespace nbppfe.CustomItems
{
    public class ITM_SweepWhistle : Item, IItemPrefab
    {
        public void Setup() =>   
            whistleSound = (SoundObject)Items.PrincipalWhistle.ToItem().item.GetComponent<ITM_PrincipalWhistle>().ReflectionGetVariable("audWhistle");

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            this.pm = pm;
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(whistleSound);
            WhistleReact(pm.transform.position);
            Destroy(base.gameObject);
            return true;
        }


        public void WhistleReact(Vector3 target)
        {
            foreach (NPC npc in pm.ec.Npcs)
            {
                if (npc.Character == Character.Sweep)
                    npc.behaviorStateMachine.ChangeState(new GottaSweep_WhistleApproach(npc.GetComponent<GottaSweep>(), target));
            }
        }

        public SoundObject whistleSound;
    }

    public class GottaSweep_WhistleApproach : GottaSweep_StateBase
    {
        protected NpcState previousState;
        protected Vector3 destination;

        public GottaSweep_WhistleApproach(GottaSweep sweep, Vector3 destination) : base(sweep, sweep)
        {
            this.destination = destination;
            npc = sweep;
            gottaSweep = sweep;
        }

        public override void Initialize()
        {
            base.Initialize();
            gottaSweep.StartSweeping();
        }

        public override void Update() =>      
            base.ChangeNavigationState(new NavigationState_TargetPosition(npc, 63, destination));
        

        public override void DestinationEmpty()
        {
            base.DestinationEmpty();
            npc.behaviorStateMachine.ChangeState(new GottaSweep_SweepingTime(npc, gottaSweep));
        }
    }

}
