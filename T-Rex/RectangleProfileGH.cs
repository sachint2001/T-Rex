using System;

using Grasshopper.Kernel;
using T_RexEngine;

namespace T_Rex
{
    public class RectangleProfileGH : GH_Component
    {
        public RectangleProfileGH()
            : base("Rectangle Profile", "Rectangle Profile",
                "Creates rectangle profile for the element",
                "T-Rex", "Concrete")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "Height", "Height of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "Width", "Width of the profile", GH_ParamAccess.item);
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
            double height = Double.NaN;
            double width = Double.NaN;
            double tolerance = Double.NaN;

            DA.GetData(0, ref name);
            DA.GetData(1, ref height);
            DA.GetData(2, ref width);
            DA.GetData(3, ref tolerance);
            
            Profile elementProfile = new Profile(name, height, width, tolerance);

            DA.SetData(0, elementProfile);
            DA.SetData(1, elementProfile.BoundarySurfaces[0]);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.RectangleProfile;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("a0b3f321-f6a9-4e9f-ac30-2b02b81326c3"); }
        }
    }
}