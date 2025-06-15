using PlusLevelFormat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Lots_o__level_types
{
    internal class LaserFieldLogic : MonoBehaviour, IButtonReceiver
    {
        public bool IsEnabled = true;
        List<GameButtonBase> ConnectedLevers = new List<GameButtonBase>();
        internal Cell CurrentCell;
        GameObject ActualLaser;
        [SerializeField]
        public PropagatedAudioManager AudMan;
        [SerializeField]
        public Direction StructureDirection;

        MapTile Icon;
        EnvironmentController Ec;

        private float timeLeftOn = 10;
        void Start()
        {
            timeLeftOn = UnityEngine.Random.Range(0.5f, 12f);
            Ec = Singleton<BaseGameManager>.Instance.Ec;
            var ThisTile = Ec.CellFromPosition(this.transform.position);
            CurrentCell = ThisTile;
            Icon = Ec.map.AddExtraTile(ThisTile.position);
            Icon.transform.rotation = StructureDirection.ToUiRotation();
            Icon.SpriteRenderer.color = Color.red;
            Icon.SpriteRenderer.sprite = BasePlugin.AssetMan.Get<Sprite>("Icon_lsrOn");
            ActualLaser = gameObject.transform.Find("lazeractual-1").gameObject;
        }

        void Update()
        {
            ActualLaser.SetActive(IsEnabled);
            
            if(!IsEnabled && timeLeftOn >= 0f)
            {
                timeLeftOn -= Time.deltaTime;
            } else if (!IsEnabled && timeLeftOn < 0)
            {
                foreach (var lever in ConnectedLevers)
                {
                    AudMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_LaserOn"));
                    lever.Set(true);
                }
                IsEnabled = true;
                timeLeftOn = 25f;
                
            }
            if (IsEnabled) Icon.SpriteRenderer.sprite = BasePlugin.AssetMan.Get<Sprite>("Icon_lsrOn");
            else Icon.SpriteRenderer.sprite = BasePlugin.AssetMan.Get<Sprite>("Icon_lsrOff");
            if (IsEnabled) Icon.SpriteRenderer.color = Color.red;
            else Icon.SpriteRenderer.color = Color.white;
            var cell = Ec.CellFromPosition(transform.position);
            var dirs = Directions.All();
            for (int i = 0; i < dirs.Count; i++){
            cell.Block(dirs[i], IsEnabled);
            var neighborCell = Ec.CellFromPosition(cell.position + dirs[i].ToIntVector2());
            if (neighborCell != null)
            {
                neighborCell.Block(dirs[i].GetOpposite(), IsEnabled);
            }
            } }

        void OnTriggerEnter(Collider other)
        {
            Entity otherEntity = other.GetComponent<Entity>();

            if (otherEntity == null || !IsEnabled) return;
            otherEntity.AddForce(new Force((otherEntity.transform.position - gameObject.transform.position).normalized, 100, -75f));
            
            AudMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_Laser"),0.5f);
        }


        public void ButtonPressed(bool val)
        {
            IsEnabled = !IsEnabled; 

            


            timeLeftOn = 25;
            if (IsEnabled) AudMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_LaserOn"));
            else AudMan.PlaySingle(BasePlugin.AssetMan.Get<SoundObject>("Aud_LaserOff"));
        }

        public void ConnectButton(GameButtonBase button)
        {
            ConnectedLevers.Add(button);
            button.SetPowered(IsEnabled);
            button.Set(true);
        }
    }
}
