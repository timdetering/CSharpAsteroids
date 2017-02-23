using System;
using System.Collections;
using System.Drawing;

namespace Asteroids
{
    /// <summary>
    /// Summary description for CShip.
    /// </summary>
    class Ship : ScreenObject
    {
        enum SHIP_STATE
        {
            WAITING,
            ALIVE,
            EXPLODING,
            DONE
        };

        SHIP_STATE state;
        const double ROTATE_SPEED = 12000 / FPS;
        int iPointThrust1;
        int iPointThrust2;
        bool bThrustOn;

        public Ship() : base(new Point(iMaxX / 2, iMaxY / 2))
        {
            bThrustOn = false;
            state = SHIP_STATE.WAITING;
        }

        public Ship(bool bAlive) : this()
        {
            state = SHIP_STATE.ALIVE;
        }

        public override void InitPoints()
        {
            const int shipWidthHalf = 100;
            const int shipHeightHalf = shipWidthHalf * 2;
            const int shipHeightInUp = (int)(shipHeightHalf * .6);
            const int shipWidthInSide = (int)(shipWidthHalf * .3);
            AddPoint(new Point(0, -shipHeightHalf));
            AddPoint(new Point(shipWidthHalf / 2, 0)); // midpoint for collisions
            AddPoint(new Point(shipWidthHalf, shipHeightHalf));
            iPointThrust1 = AddPoint(new Point(shipWidthInSide, shipHeightInUp));
            iPointThrust2 = AddPoint(new Point(-shipWidthInSide, shipHeightInUp));
            AddPoint(new Point(-shipWidthHalf, shipHeightHalf));
            AddPoint(new Point(-shipWidthHalf / 2, 0)); // midpoint for collisions
        }

        public bool Hyperspace()
        {
            bool bSafeHyperspace = (rndGen.Next(10) != 1);
            currLoc.X = (rndGen.Next((int)(iMaxX * .8))) + (int)(iMaxX * .1);
            currLoc.Y = (rndGen.Next((int)(iMaxY * .8))) + (int)(iMaxY * .1);
            return bSafeHyperspace;
        }

        public void Explode()
        {
            state = SHIP_STATE.EXPLODING;
            velocityX = velocityY = 0;
        }

        public bool IsAlive()
        {
            return (state == SHIP_STATE.ALIVE);
        }

        public void DecayThrust()
        {
            bThrustOn = false;

            velocityX = velocityX * (1 - 1 / FPS);
            velocityY = velocityY * (1 - 1 / FPS);
        }

        public void Thrust()
        {
            bThrustOn = true;

            double SinVal = Math.Sin(radians);
            double CosVal = Math.Cos(radians);
            double addThrust = 90 / FPS;
            double maxThrustSpeed = 5000 / FPS;
            double incX, incY;

            incX = -(addThrust * SinVal);
            incY = addThrust * CosVal;

            velocityX += incX;
            if (velocityX > maxThrustSpeed)
            {
                velocityX = maxThrustSpeed;
            }
            if (velocityX < -maxThrustSpeed)
            {
                velocityX = -maxThrustSpeed;
            }
            velocityY += incY;
            if (velocityY > maxThrustSpeed)
            {
                velocityY = maxThrustSpeed;
            }
            if (velocityY < -maxThrustSpeed)
            {
                velocityY = -maxThrustSpeed;
            }
            PlaySound("THRUST.WAV");
        }

        public void RotateLeft()
        {
            base.Rotate(-ROTATE_SPEED);
        }

        public void RotateRight()
        {
            base.Rotate(ROTATE_SPEED);
        }

        public new void Draw(ScreenCanvas sc, int iPictX, int iPictY)
        {
            switch (state)
            {
                case SHIP_STATE.ALIVE:
                    base.Draw(sc, iPictX, iPictY);
                    if (bThrustOn)
                    {
                        // We have points transformed 
                        // so...  we know where the bottom of the ship is
                        ArrayList alPoly = new ArrayList();
                        alPoly.Capacity = 3;
                        alPoly.Add(pointsTransformed[iPointThrust1]);
                        alPoly.Add(pointsTransformed[iPointThrust2]);
                        int iThrustSize = rndGen.Next(200) + 100; // random thrust effect
                        alPoly.Add(new Point((((Point)pointsTransformed[iPointThrust1]).X + ((Point)pointsTransformed[iPointThrust2]).X) / 2 + (int)(iThrustSize * Math.Sin(radians)),
                                             (((Point)pointsTransformed[iPointThrust1]).Y + ((Point)pointsTransformed[iPointThrust2]).Y) / 2 + (int)(-iThrustSize * Math.Cos(radians))));
                        // Draw thrust directly to ScreenCanvas
                        // it's not really part of the ship object
                        DrawPolyToSC(alPoly, sc, iPictX, iPictY, GetRandomFireColor());
                    }
                    break;
            }
        }
    }
}