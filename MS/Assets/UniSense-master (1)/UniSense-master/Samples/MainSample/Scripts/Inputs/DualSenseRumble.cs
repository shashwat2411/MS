

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// Component to set the DualSense rumble, aka motor speeds.
    /// <para>Set the <see cref="LeftRumble"/> and/or <see cref="RightRumble"/> Properties.</para>
    /// </summary>
    public class DualSenseRumble : AbstractDualSenseBehaviour
    {
       // GameObject maincamera;

        /// <summary>
        /// Speed of the low-frequency (left) motor. 
        /// Normalized [0..1] value with 1 indicating maximum speed 
        /// and 0 indicating the motor is turned off.
        /// </summary>
        //public float LeftRumble { get; set; }

        /// <summary>
        /// Speed of the high-frequency (right) motor. 
        /// Normalized [0..1] value with 1 indicating maximum speed 
        /// and 0 indicating the motor is turned off.
        /// </summary>
        //public float RightRumble { get; set; }

        private void Start()
        {
            //maincamera = GameObject.Find("Main Camera");
        }


        private void Update()
        {
           // Vector2 rumble = maincamera.GetComponent<CameraBrain>().SetGamePadMotorSpeed();

            //  DualSense
            //DualSense?.SetMotorSpeeds(rumble.x, rumble.y);

            //  InputSystem
            //Gamepad.current?.SetMotorSpeeds(rumble.x, rumble.y);

           // Debug.Log(rumble.x + ":::::" + rumble.y);

        }

        //public void UpdateMotorSpeeds()
        //    =>
        //    DualSense?.SetMotorSpeeds(LeftRumble, RightRumble);

       
    }
}
