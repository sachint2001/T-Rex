using System;

using Grasshopper.Kernel;
using T_RexEngine;

namespace T_Rex
{
    public class TProfileGH : GH_Component
    {
        public TProfileGH()
            : base("T Profile", "T Profile",
                "Creates T profile for the element",
                "T-Rex", "Concrete")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "Height", "Height of the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Flange Height", "Flange Height", "Flange height for the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Flange Width", "Flange Width", "Flange width for the profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Web Width", "Web Width", "Web width for the profile", GH_ParamAccess.item);
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
            double flangeHeight = Double.NaN;
            double flangeWidth = Double.NaN;
            double webWidth = Double.NaN;
            double tolerance = Double.NaN;

            DA.GetData(0, ref name);
            DA.GetData(1, ref height);
            DA.GetData(2, ref flangeHeight);
            DA.GetData(3, ref flangeWidth);
            DA.GetData(4, ref webWidth);
            DA.GetData(5, ref tolerance);
            
            Profile elementProfile = new Profile(name, 0, height, flangeHeight, webWidth, flangeWidth, tolerance);

            DA.SetData(0, elementProfile);
            DA.SetData(1, elementProfile.BoundarySurfaces[0]);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Profile;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c2d76536-d748-4031-8722-8b849a0a4002"); }
        }
    }
}