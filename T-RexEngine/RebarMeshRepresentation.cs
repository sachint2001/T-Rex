using System.Collections.Generic;
using Rhino.Geometry;

namespace T_RexEngine
{
    public class RebarMeshRepresentation
    {
        public static List<Point3d> CreateSectionPoints(int diameter)
        {
            List<Point3d> sectionPoints = new List<Point3d>
            {
                new Point3d(0.433*diameter, 0.250*diameter, 0),
                new Point3d(0.250*diameter, 0.433*diameter, 0),
                new Point3d(0.000, 0.500*diameter, 0),
                new Point3d(-0.250*diameter, 0.433*diameter, 0),
                new Point3d(-0.433*diameter, 0.250*diameter, 0),
                new Point3d(-0.500*diameter, 0.000, 0),
                new Point3d(-0.433*diameter, -0.250*diameter, 0),
                new Point3d(-0.250*diameter, -0.433*diameter, 0),
                new Point3d(0.000, -0.500*diameter, 0),
                new Point3d(0.250*diameter, -0.433*diameter, 0),
                new Point3d(0.433*diameter, -0.250*diameter, 0),
                new Point3d(0.500*diameter, 0.000, 0)
            };

            return sectionPoints;
        }

        public static List<int[]> StartEndMeshFaceIndexes(List<Point3d> rebarMeshPoints)
        {
            List<int[]> faceIntegers = new List<int[]>();

            for (int i = 0; i < 11; i++)
            {
                faceIntegers.Add(new []{0, i+1, i+2});
            }
            faceIntegers.Add(new[] { 0, 12, 1 });

            for (int i = rebarMeshPoints.Count - 1; i > rebarMeshPoints.Count - 12; i--)
            {
                faceIntegers.Add(new[] { rebarMeshPoints.Count - 1, i - 1, i - 2 });
            }
            faceIntegers.Add(new[] { rebarMeshPoints.Count - 1, rebarMeshPoints.Count - 13, rebarMeshPoints.Count - 2 });

            return faceIntegers;
        }

        public static List<int[]> MiddleMeshFaceIndexes(List<Point3d> rebarMeshPoints)
        {
            List<int[]> faceIntegers = new List<int[]>();

            for (int i = 1; i < rebarMeshPoints.Count - 2 - 12; i += 12)
            {
                for (int j = 0; j < 11; j++)
                {
                    faceIntegers.Add(new[] { j + i, j + 1 + i, j + 12 + 1 + i, j + 12 + i });
                }
                faceIntegers.Add(new[] { i, i + 12, i + 12 + 11, i + 11 });
            }

            return faceIntegers;
        }

        public static Mesh CreateRebarMesh(List<Point3d> rebarMeshPoints)
        {
            Mesh rebarMesh = new Mesh();

            foreach (var point in rebarMeshPoints)
            {
                rebarMesh.Vertices.Add(point);
            }

            List<int[]> startEndMeshFacesIntegers = StartEndMeshFaceIndexes(rebarMeshPoints);
            foreach (var intTable in startEndMeshFacesIntegers)
            {
                rebarMesh.Faces.AddFace(intTable[0], intTable[1], intTable[2]);
            }

            List<int[]> middleMeshFaceIntegers = MiddleMeshFaceIndexes(rebarMeshPoints);
            foreach (var intTable in middleMeshFaceIntegers)
            {
                rebarMesh.Faces.AddFace(intTable[0], intTable[1], intTable[2], intTable[3]);
            }

            rebarMesh.Normals.ComputeNormals();
            rebarMesh.Compact();

            return rebarMesh;
        }

        public static List<Point3d> CreateRebarMeshPoints(List<Point3d> sectionPoints,
            List<Point3d> curveDivisionPoints)
        {
            List<Point3d> rebarMeshPoints = new List<Point3d>();
            Vector3d workVector = new Vector3d();
            Plane workPlane;

            rebarMeshPoints.Add(curveDivisionPoints[0]);

            for (int i = 0; i < curveDivisionPoints.Count - 1; i++)
            {
                workVector = new Vector3d
                (
                    curveDivisionPoints[i + 1].X - curveDivisionPoints[i].X,
                    curveDivisionPoints[i + 1].Y - curveDivisionPoints[i].Y,
                    curveDivisionPoints[i + 1].Z - curveDivisionPoints[i].Z
                );
                workPlane = new Plane(curveDivisionPoints[i], workVector);
                rebarMeshPoints.AddRange(MoveXyPointsToAnotherPlane(sectionPoints, workPlane));
            }

            workPlane = new Plane(curveDivisionPoints[curveDivisionPoints.Count - 1], workVector);
            rebarMeshPoints.AddRange(MoveXyPointsToAnotherPlane(sectionPoints, workPlane));

            rebarMeshPoints.Add(curveDivisionPoints[curveDivisionPoints.Count - 1]);

            return rebarMeshPoints;
        }

        public static List<Point3d> MoveXyPointsToAnotherPlane(List<Point3d> pointsToMove, Plane destinationPlane)
        {
            List<Point3d> movedPoints = new List<Point3d>(); 

            Transform changeBasis = Transform.ChangeBasis(destinationPlane, Plane.WorldXY);

            foreach (var point in pointsToMove)
            {
                point.Transform(changeBasis);
                movedPoints.Add(point);
            }

            return movedPoints;
        }
    }
}
