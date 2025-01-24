using UniSense;
using UnityEngine;

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// Component to set the DualSense Triggers.
    /// </summary>
    public class DualSenseTrigger : AbstractDualSenseBehaviour
    {

        PlayerManager player;
        float lowRange;
        float middleRange;
        float highRange;

        

        [SerializeField]
        [Range(0, 1)]
        float Lv1StartPosition;

        [SerializeField]
        [Range(0, 1)]
        float Lv1ContinuousForce;

        [SerializeField]
        [Range(0, 1)]
        float Lv2StartPosition;

        [SerializeField]
        [Range(0, 1)]
        float Lv2ContinuousForce;

        [SerializeField]
        [Range(0, 1)]
        float Lv3StartPosition;

        [SerializeField]
        [Range(0, 1)]
        float Lv3ContinuousForce;

        [SerializeField]
        [Range(0, 1)]
        float EffectEndForce;

        [SerializeField]
        [Range(0, 1)]
        float EffectFrequency;



        #region Properties for Left Trigger
        public int LeftTriggerEffectType
        {
            get => (int)leftTriggerState.EffectType;
            set => leftTriggerState.EffectType = SetTriggerEffectType(value);
        }

        #region Continuous Resistance Properties
        public float LeftContinuousForce
        {
            get => leftTriggerState.Continuous.Force;
            set => leftTriggerState.Continuous.Force = (byte)(value * 255);
        }

        public float LeftContinuousStartPosition
        {
            get => leftTriggerState.Continuous.StartPosition;
            set => leftTriggerState.Continuous.StartPosition = (byte)(value * 255);
        }
        #endregion

        #region Section Resistance Properties
        public float LeftSectionForce
        {
            get => leftTriggerState.Section.Force;
            set => leftTriggerState.Section.Force = (byte)(value * 255);
        }

        public float LeftSectionStartPosition
        {
            get => leftTriggerState.Section.StartPosition;
            set => leftTriggerState.Section.StartPosition = (byte)(value * 255);
        }

        public float LeftSectionEndPosition
        {
            get => leftTriggerState.Section.EndPosition;
            set => leftTriggerState.Section.EndPosition = (byte)(value * 255);
        }
        #endregion

        #region EffectEx Properties
        public float LeftEffectStartPosition
        {
            get => leftTriggerState.EffectEx.StartPosition;
            set => leftTriggerState.EffectEx.StartPosition = (byte)(value * 255);
        }

        public float LeftEffectBeginForce
        {
            get => leftTriggerState.EffectEx.BeginForce;
            set => leftTriggerState.EffectEx.BeginForce = (byte)(value * 255);
        }

        public float LeftEffectMiddleForce
        {
            get => leftTriggerState.EffectEx.MiddleForce;
            set => leftTriggerState.EffectEx.MiddleForce = (byte)(value * 255);
        }

        public float LeftEffectEndForce
        {
            get => leftTriggerState.EffectEx.EndForce;
            set => leftTriggerState.EffectEx.EndForce = (byte)(value * 255);
        }

        public float LeftEffectFrequency
        {
            get => leftTriggerState.EffectEx.Frequency;
            set => leftTriggerState.EffectEx.Frequency = (byte)(value * 255);
        }

        public bool LeftEffectKeepEffect
        {
            get => leftTriggerState.EffectEx.KeepEffect;
            set => leftTriggerState.EffectEx.KeepEffect = value;
        }
        #endregion
        #endregion

        #region Properties for Right Trigger
        public int RightTriggerEffectType
        {
            get => (int)rightTriggerState.EffectType;
            set => rightTriggerState.EffectType = SetTriggerEffectType(value);
        }

        #region Continuous Resistance Properties
        public float RightContinuousForce
        {
            get => rightTriggerState.Continuous.Force;
            set => rightTriggerState.Continuous.Force = (byte)(value * 255);
        }

        public float RightContinuousStartPosition
        {
            get => rightTriggerState.Continuous.StartPosition;
            set => rightTriggerState.Continuous.StartPosition = (byte)(value * 255);
        }
        #endregion

        #region Section Resistance Properties
        public float RightSectionForce
        {
            get => rightTriggerState.Section.Force;
            set => rightTriggerState.Section.Force = (byte)(value * 255);
        }

        public float RightSectionStartPosition
        {
            get => rightTriggerState.Section.StartPosition;
            set => rightTriggerState.Section.StartPosition = (byte)(value * 255);
        }

        public float RightSectionEndPosition
        {
            get => rightTriggerState.Section.EndPosition;
            set => rightTriggerState.Section.EndPosition = (byte)(value * 255);
        }
        #endregion

        #region EffectEx Properties
        public float RightEffectStartPosition
        {
            get => rightTriggerState.EffectEx.StartPosition;
            set => rightTriggerState.EffectEx.StartPosition = (byte)(value * 255);
        }

        public float RightEffectBeginForce
        {
            get => rightTriggerState.EffectEx.BeginForce;
            set => rightTriggerState.EffectEx.BeginForce = (byte)(value * 255);
        }

        public float RightEffectMiddleForce
        {
            get => rightTriggerState.EffectEx.MiddleForce;
            set => rightTriggerState.EffectEx.MiddleForce = (byte)(value * 255);
        }

        public float RightEffectEndForce
        {
            get => rightTriggerState.EffectEx.EndForce;
            set => rightTriggerState.EffectEx.EndForce = (byte)(value * 255);
        }

        public float RightEffectFrequency
        {
            get => rightTriggerState.EffectEx.Frequency;
            set => rightTriggerState.EffectEx.Frequency = (byte)(value * 255);
        }

        public bool RightEffectKeepEffect
        {
            get => rightTriggerState.EffectEx.KeepEffect;
            set => rightTriggerState.EffectEx.KeepEffect = value;
        }
        #endregion
        #endregion

        public DualSenseTriggerState leftTriggerState;
        public DualSenseTriggerState rightTriggerState;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerManager>();
            lowRange = player.playerAttack.lowRange;
            middleRange = player.playerAttack.middleRange;
            highRange = player.playerAttack.highRange;
        }

        private void Awake()
        {
           

            leftTriggerState = new DualSenseTriggerState
            {
                EffectType = DualSenseTriggerEffectType.ContinuousResistance,
                EffectEx = new DualSenseEffectExProperties(),
                Section = new DualSenseSectionResistanceProperties(),
                Continuous = new DualSenseContinuousResistanceProperties()
            };

            rightTriggerState = new DualSenseTriggerState
            {
                EffectType = DualSenseTriggerEffectType.ContinuousResistance,
                EffectEx = new DualSenseEffectExProperties(),
                Section = new DualSenseSectionResistanceProperties(),
                Continuous = new DualSenseContinuousResistanceProperties()
            };

            RightEffectKeepEffect = false;
        }

        private void Update()
        {
            var state = new DualSenseGamepadState
            {
                LeftTrigger = leftTriggerState,
                RightTrigger = rightTriggerState
            };
            DualSense?.SetGamepadState(state);

            if (DualSense != null)
            {

               // Debug.Log(DualSense.rightTrigger.value);


                float charge = player.playerData.charge;
                float maxCharge = player.playerData.maxChargeTime;

                float lowRangeCharge = maxCharge * lowRange / 100.0f;
                float middleRangeCharge = maxCharge * middleRange / 100.0f;
                float highRangeCharge = maxCharge * highRange / 100.0f;

                //Level 1
                if (charge <= maxCharge * lowRange / 100.0f)
                {

                    rightTriggerState.EffectType = DualSenseTriggerEffectType.ContinuousResistance;
                    RightContinuousStartPosition = Lv1StartPosition;
                    rightTriggerState.Continuous.Force = (byte)(Lv1ContinuousForce * 255);

                }
                //Level 2
                if (charge > lowRangeCharge && charge <= middleRangeCharge)
                {
                    rightTriggerState.EffectType = DualSenseTriggerEffectType.ContinuousResistance;
                    RightContinuousStartPosition = Lv2StartPosition;
                    rightTriggerState.Continuous.Force = (byte)(Lv2ContinuousForce * 255);
                }
                //Level 3
                if (charge <= highRangeCharge && charge > middleRangeCharge)
                {
                    rightTriggerState.EffectType = DualSenseTriggerEffectType.ContinuousResistance;
                    RightContinuousStartPosition = Lv3StartPosition;
                    rightTriggerState.Continuous.Force = (byte)(Lv3ContinuousForce * 255);
                }
                //Level Max
                if (charge > maxCharge * highRange / 100.0f)
                {
                    rightTriggerState.EffectType = DualSenseTriggerEffectType.EffectEx;
                    RightEffectStartPosition = 1.0f;
                    rightTriggerState.EffectEx.EndForce = (byte)(EffectEndForce * 255);
                    rightTriggerState.EffectEx.Frequency = (byte)(EffectFrequency * 255);
                    RightEffectKeepEffect = true;
                }


            }
        }

        private DualSenseTriggerEffectType SetTriggerEffectType(int index)
        {
            if (index == 0) return DualSenseTriggerEffectType.ContinuousResistance;
            if (index == 1) return DualSenseTriggerEffectType.SectionResistance;
            if (index == 2) return DualSenseTriggerEffectType.EffectEx;

            return DualSenseTriggerEffectType.NoResistance;
        }

        
    }
}
