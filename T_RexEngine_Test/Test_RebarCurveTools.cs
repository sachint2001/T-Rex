using System;
using System.Collections.Generic;
using System.ComponentModel;
using Eto.Forms;
using Rhino.Geometry;
using T_RexEngine;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace T_RexEngine_Test
{
    [Collection("Rhino Collection")]
    public class Test_RebarCurveTools
    {
        [Fact]
        public void Test_ExplodeIntoSegments_3PointCurve_2Segments()
        {
            Curve rebarCurve = new PolylineCurve(new List<Point3d>
            {
                new Point3d(0, 0, 0), new Point3d(10, 0, 0), new Point3d(0, 10, 0)
            });
            rebarCurve = Curve.CreateFilletCornersCurve(rebarCurve, 10, 0.001, 0.1);

            List<Curve> segmentsToTest = RebarCurveTools.ExplodeIntoSegments(rebarCurve);

            Assert.Equal(2, segmentsToTest.Count);
        }

        [Fact]
        public void Test_ExplodeIntoSegments_2PointLine_1Segment()
        {
            Curve rebarCurve = new PolylineCurve(new List<Point3d>
            {
                new Point3d(0, 0, 0), new Point3d(10, 0, 0)
            });
            rebarCurve = Curve.CreateFilletCornersCurve(rebarCurve, 10, 0.001, 0.1);

            List<Curve> segmentsToTest = RebarCurveTools.ExplodeIntoSegments(rebarCurve);

            Assert.Single(segmentsToTest);
        }

        [Fact]
        public void Test_GetParameters_1Segment_ActualParameters()
        {
            Curve rebarCurve = new LineCurve(
                new Point3d(0, 0, 0),
                new Point3d(10, 0, 0)
            );

            List<Curve> segments = new List<Curve> { rebarCurve };

            List<double> parameters = RebarCurveTools.GetParameters(segments, rebarCurve);

            Assert.Equal(0, parameters[0]);
            Assert.Equal(10, parameters[1]);
            Assert.Equal(2, parameters.Count);

        }

        [Fact]
        public void Test_GetParameters_2Segment_ActualParameters()
        {
            Curve rebarCurve = new PolylineCurve(new List<Point3d>
            {
                new Point3d(0, 0, 0),
                new Point3d(10, 0, 0),
                new Point3d(10, 10, 0)
            });

            List<Curve> segments = new List<Curve>
            {
                new PolylineCurve(new List<Point3d>
                {
                    new Point3d(0, 0, 0),
                    new Point3d(10, 0, 0)
                }),
                new PolylineCurve(new List<Point3d>
                {
                    new Point3d(10, 0, 0),
                    new Point3d(10, 10, 0)
                })
            };

            List<double> parameters = RebarCurveTools.GetParameters(segments, rebarCurve);

            Assert.Equal(0, parameters[0]);
            Assert.Equal(10, parameters[1]);
            Assert.Equal(20, parameters[2]);
            Assert.Equal(3, parameters.Count);

        }

        [Fact]
        public void Test_GetParameters_3Segments2Lines1Arc_ActualParameters()
        {
            Curve rebarCurve = new PolylineCurve(new List<Point3d>
            {
                new Point3d(0, 0, 0), new Point3d(10, 0, 0), new Point3d(10, 10, 0)
            });
            rebarCurve = Curve.CreateFilletCornersCurve(rebarCurve, 2, 0.001, 0.01);

            List<Curve> segmentsToTest = RebarCurveTools.ExplodeIntoSegments(rebarCurve);

            List<double> parameters = RebarCurveTools.GetParameters(segmentsToTest, rebarCurve);
            List<double> roundedParameters = new List<double>();
            foreach (var param in parameters)
            {
                roundedParameters.Add(Math.Round(param, 2));
            }

            List<double> expectedParameters = new List<double>
            {
                0, 8, 8.31, 8.63, 8.94, 9.26, 9.57, 9.88, 10.2, 10.51, 10.83, 11.14, 19.14
            };

            for (int i = 0; i < expectedParameters.Count; i++)
            {
                Assert.Equal(expectedParameters[i], roundedParameters[i]);
            }

            Assert.Equal(expectedParameters.Count, parameters.Count);
        }

        [Fact]
        public void Test_GetDivisionPlanesForRebarCurve_1LineCurve_2Planes()
        {
            Curve rebarCurve = new LineCurve(
                new Point3d(0, 0, 0),
                new Point3d(10, 0, 0)
            );
            List<Plane> testDivision = RebarCurveTools.GetDivisionPlanesForRebarCurve
                (rebarCurve, new List<double> {0, 10});

            Assert.Equal(2, testDivision.Count);
        }

        [Fact]
        public void Test_GetDivisionPlanesForRebarCurve_1CurveWith3Vertices_3Planes()
        {
            Curve rebarCurve = new PolylineCurve(new List<Point3d>
            {
                new Point3d(0, 0, 0), new Point3d(10, 0, 0), new Point3d(10, 10, 0)
            });
            List<Plane> testDivision = RebarCurveTools.GetDivisionPlanesForRebarCurve
                (rebarCurve, new List<double> { 0, 10, 20 });

            Assert.Equal(3, testDivision.Count);
        }
    }
}
