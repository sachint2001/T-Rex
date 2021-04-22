using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using T_RexEngine;

namespace T_Rex
{
    public class PolylineProfileGH : GH_Component
    {
        public PolylineProfileGH()
            : base("Polyline Profile", "Polyline Profile",
                "Creates polyline profile for the element",
                "T-Rex", "Concrete")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the profile", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Points", "Points that define a section", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tolerance", "Tolerance setting for Brep creation",
                GH_ParamAccess.item, 0.0001);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Profile", "Profile", "Created profile", GH_ParamAccess.item);
            pManager.AddBrepParameter("Brep", "Brep", "Brep of profile", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = String.Empty;
            List<Point3d> points = new List<Point3d>();
            double tolerance = Double.NaN;

            DA.GetData(0, ref name);
            DA.GetDataList(1, points);
            DA.GetData(2, ref tolerance);
            
            Profile elementProfile = new Profile(name, points, tolerance);

            DA.SetData(0, elementProfile);
            DA.SetData(1, elementProfile.BoundarySurfaces[0]);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.PolylineProfile;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9bb0b854-4319-43aa-88b8-5c4250ff0a37"); }
        }
    }
}