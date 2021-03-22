﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using T_RexEngine;

namespace T_Rex
{
    public class ProfileGH : GH_Component
    {
        public ProfileGH()
            : base("Profile", "Profile",
                "Creates profile for the element",
                "T-Rex", "Concrete")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Points", "Points that define a section", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "Tolerance", "Tolerance setting for Brep creation",
                GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Profile", "Profile", "Created profile", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve of profile", GH_ParamAccess.item);
            pManager.AddBrepParameter("Brep", "Brep", "Brep of profile", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> points = new List<Point3d>();
            double tolerance = Double.NaN;

            DA.GetDataList(0, points);
            DA.GetData(1, ref tolerance);
            
            ElementProfile elementProfile = new ElementProfile(points, tolerance);

            DA.SetData(0, elementProfile);
            DA.SetData(1, elementProfile.ProfileCurve);
            DA.SetDataList(2, elementProfile.BoundarySurfaces);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("67613946-8903-4845-aa5b-92c3e908f530"); }
        }
    }
}