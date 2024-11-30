using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        public override void OnCretae()
        {
            base.OnCretae();
            Debug.Log("BattleWorld  OnCretae>>>");
            ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO + "1000", null);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);

        }

    }

}