namespace SpaceBattle.Lib.Command
{
    public static class CollisionDetector
    {
        public static bool CheckCollision(
            Vector start1, Vector end1, IReadOnlyList<Circle> circles1,
            Vector start2, Vector end2, IReadOnlyList<Circle> circles2,
            double dt = 1.0)
        {
            foreach (var c1 in circles1)
            {
                foreach (var c2 in circles2)
                {
                    double s1x = start1.Coordinates[0] + c1.Center.Coordinates[0];
                    double s1y = start1.Coordinates[1] + c1.Center.Coordinates[1];
                    double e1x = end1.Coordinates[0] + c1.Center.Coordinates[0];
                    double e1y = end1.Coordinates[1] + c1.Center.Coordinates[1];
                    double s2x = start2.Coordinates[0] + c2.Center.Coordinates[0];
                    double s2y = start2.Coordinates[1] + c2.Center.Coordinates[1];
                    double e2x = end2.Coordinates[0] + c2.Center.Coordinates[0];
                    double e2y = end2.Coordinates[1] + c2.Center.Coordinates[1];

                    if (SweptCircleIntersection(s1x, s1y, e1x, e1y, c1.Radius,
                                                s2x, s2y, e2x, e2y, c2.Radius, dt))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool SweptCircleIntersection(
            double a0x, double a0y, double a1x, double a1y, double ra,
            double b0x, double b0y, double b1x, double b1y, double rb,
            double dt)
        {
            double relPosX = a0x - b0x;
            double relPosY = a0y - b0y;
            double relVelX = (a1x - a0x - (b1x - b0x)) / dt;
            double relVelY = (a1y - a0y - (b1y - b0y)) / dt;
            double sumR = ra + rb;

            double a = relVelX * relVelX + relVelY * relVelY;
            double b = 2 * (relPosX * relVelX + relPosY * relVelY);
            double c = relPosX * relPosX + relPosY * relPosY - sumR * sumR;

            if (Math.Abs(a) < 1e-10)
            {
                return c <= 0;
            }

            double disc = b * b - 4 * a * c;
            if (disc < 0)
            {
                return false;
            }

            double t1 = (-b - Math.Sqrt(disc)) / (2 * a);
            double t2 = (-b + Math.Sqrt(disc)) / (2 * a);
            if (t1 > t2)
            {
                (t1, t2) = (t2, t1);
            }

            return t1 <= dt && t2 >= 0;
        }
    }
}
