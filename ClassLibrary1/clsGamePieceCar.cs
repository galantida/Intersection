﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public enum ShifterPosition { reverse, neutral, drive }

    public class clsGamePieceCar : clsBaseGamePiece, intGamePiece
    {
        // specifications
        public float acceleration = 0.2f; // force to add in the direction of the transmissions
        public float breaking = 0.5f; // creates additional drag on the dirtion of force
        public float handling = 0.01f; // .05 was good
        public float weight = 1000;
        public float topSpeed = 0.2f; // .2 was good
        public float rollingResistance = 0.01f; // default resistance caused by the wheels

        // inputs
        public float pedals { get; set; }
        public float steering { get; set; }
        public float shifter { get; set; }

        // car status
        public Vector2 direction { get; set; }

        // driver either AI or HUman
        public intDriver driver;


        public clsGamePieceCar(intDriver driver, Vector2 location, Vector2 direction, Vector2 velocity) :base(location, velocity)
        {
            this.driver = driver;
            base.gamePieceType = GamePieceType.car;
            base.mass = this.weight;
            this.direction = direction; // car direction
        }

        new public void update(clsWorld world)
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 10)
            {
                // update AI
                driver.update(this);

                /**************************************** 
                        steering and direction
                ****************************************/

                // change facing direction based on steering wheel position
                handling = 0.002f;
                Vector2 existingDirection = new Vector2(direction.X, direction.Y); // copy existing direction
                float rotation = (handling * steering) * deltaTime; // calculate new car rotation based on handling steering wheel and time
                if (rotation != 0)
                {
                    direction = existingDirection.Rotate(rotation); // rotate car
                }
               

                /**************************************** 
                        acceleration and force
                ****************************************/
                if (pedals > 0)
                {
                    if (this.velocity.Length() < this.topSpeed)
                    {
                        // add force based on acceleration and transmission direction
                        this.applyForce((direction * shifter) * acceleration, deltaTime);
                    }
                    else 
                    {
                        // top speed
                        Vector2 newVelocity = velocity;
                        newVelocity.Normalize();
                        newVelocity *= topSpeed;
                        velocity = newVelocity;
                    }
                }

               

                /**************************************** 
                        breaking and resistance
                ****************************************/
                float totalResistance = rollingResistance; // standard rolling resistance
                if (pedals < 0)
                {
                    // add resistance direction adn transmission are irrelevant
                    totalResistance += breaking;
                }
                // resistance is always applied
                applyResistance(totalResistance, deltaTime);

                base.update(world);
            }
        }
    }


}
