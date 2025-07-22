using System.Collections;
using System.Collections.Generic;
using Game.Core.Board;
using Game.Core.Line;
using Game.Helpers;
using UnityEngine;

namespace Game.Ultis
{
    public class GameHelper : Singleton<GameHelper>
    {
        [SerializeField] private BoardController boardController;
        public BoardController BoardController => boardController;
        [SerializeField] private LineBlockController lineBlockController;
        public LineBlockController LineBlockController => lineBlockController;

        [SerializeField] SkinBlockController skinBlockController;
        public SkinBlockController SkinBlockController { get => skinBlockController; }

        [SerializeField] GamePoolers gamePoolers;
        public GamePoolers GamePoolers { get => gamePoolers; }

        [SerializeField] private PointFloatingController pointFloatingController;
        public PointFloatingController PointFloatingController => pointFloatingController;

        [SerializeField] ComboTextController comboTextController;
        public ComboTextController ComboTextController { get => comboTextController; }
        [SerializeField] ValueDisplayUI valueDisplayUI;
        public ValueDisplayUI ValueDisplayUI { get => valueDisplayUI; }
        [SerializeField] Poolers poolers;
        public Poolers Poolers { get => poolers; }
        [SerializeField] private GamePlayUI gamePlayUI;
        public GamePlayUI GamePlayUI { get => gamePlayUI; }

        [SerializeField] private EffectPooler effectPooler;
        public EffectPooler EffectPooler => effectPooler;
    }
}
