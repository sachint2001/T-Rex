using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace T_RexEngine
{
    public class  RebarCurveTools
    {
        public static List<Curve> ExplodeIntoSegments(Curve curveToDivideIntoSegments)
        {
            List<Curve> segmentsAsList = new List<Curve>();

            if (curveToDivideIntoSegments is PolyCurve polyCurve)
            {
                Curve[] segments = polyCurve.Explode();
                segmentsAsList.AddRange(segments);
            }
            else
            {
                segmentsAsList.Add(curveToDivideIntoSegments);
            }

            return segmentsAsList;
        }

        public static List<Plane> GetDivisionPlanesForRebarCurve(Curve wholeCurve, List<double> parameters)
        {
            List<Plane> planesOfDivision = new List<Plane>();

            foreach (var param in parameters)
            {
                wholeCurve.PerpendicularFrameAt(param, out var currentPlane);
                planesOfDivision.Add(currentPlane);
            }

            return planesOfDivision;
        }

        public static List<double> GetParameters(List<Curve> segments, Curve wholeCurve)
        {
            List<double> parameters = new List<double>();

            double domainBehindCurrent = 0;

            foreach (var segment in segments)
            {
                if (segment.IsArc())
                {
                    double arcDomain = segment.Domain[1];
                    for (int i = 0; i < 10; i++)
                    {
                        parameters.Add(domainBehindCurrent + i * arcDomain / 10);
                    }

                    domainBehindCurrent += segment.Domain[1];
                }
                else
                {
                    parameters.Add(domainBehindCurrent);
                    domainBehindCurrent += segment.Domain[1];
                }
            }

            parameters.Add(wholeCurve.Domain[1]);

            return parameters;
        }
    }
}
