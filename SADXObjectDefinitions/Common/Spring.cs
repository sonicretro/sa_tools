using System.Collections.Generic;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;
using SonicRetro.SAModel.SAEditorCommon.SETEditing;

namespace SADXObjectDefinitions.Common
{
	public abstract class SpringBase : ObjectDefinition
	{
		protected NJS_OBJECT model;
		protected Mesh[] meshes;
		protected Mesh renderAssist;
		public static NJS_MATERIAL Material { get; set; }
		public const float AssistLengthScale = 45f;

		public override HitResult CheckHit(SETItem item, Vector3 Near, Vector3 Far, Viewport Viewport, Matrix Projection, Matrix View, MatrixStack transform)
		{
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateObject(item.Rotation);
			HitResult result = model.CheckHit(Near, Far, Viewport, Projection, View, transform, meshes);
			transform.Pop();
			return result;
		}

		public override List<RenderInfo> Render(SETItem item, Device dev, EditorCamera camera, MatrixStack transform)
		{
			List<RenderInfo> result = new List<RenderInfo>();
			transform.Push();
			transform.NJTranslate(item.Position);
			transform.NJRotateObject(item.Rotation);
			result.AddRange(model.DrawModelTree(dev, transform, ObjectHelper.GetTextures("OBJ_REGULAR"), meshes));
			if (item.Selected)
			{
				result.AddRange(model.DrawModelTreeInvert(dev, transform, meshes));

				transform.Push();
				transform.NJTranslate(0, (item.Scale.Y * AssistLengthScale) + 5, 0);
				transform.NJScale(1, item.Scale.Y * AssistLengthScale, 1);
				result.Add(new RenderInfo(renderAssist, 0, transform.Top, Material, null, FillMode.Solid, new BoundingSphere(item.Position, item.Scale.Y * AssistLengthScale)));
				transform.Pop();
			}
			transform.Pop();
			return result;
		}

		private PropertySpec[] customProperties = new PropertySpec[] {
			new PropertySpec("Disable Timer", typeof(float), "Extended", null, null, (o) => o.Scale.X, (o, v) => o.Scale.X = (float)v),
			new PropertySpec("Speed", typeof(float), "Extended", null, null, (o) => o.Scale.Y, (o, v) => o.Scale.Y = (float)v)
		};

		public override PropertySpec[] CustomProperties { get { return customProperties; } }
	}

	public class Spring : SpringBase
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Common/Spring/Ground.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);

			if(renderAssist == null) 
			{
				renderAssist = Mesh.Box(dev, 2f, 2f, 2f);
			}

			if(Material == null)
			{
				Material = new NJS_MATERIAL
				{
					DiffuseColor = Color.FromArgb(200, Color.Purple),
					SpecularColor = Color.Black,
					UseAlpha = true,
					DoubleSided = false,
					Exponent = 10,
					IgnoreSpecular = false,
					UseTexture = false
				};
			}
		}

		public override string Name { get { return "Ground Spring"; } }
	}

	public class SpringB : SpringBase
	{
		public override void Init(ObjectData data, string name, Device dev)
		{
			model = ObjectHelper.LoadModel("Objects/Common/Spring/Air.sa1mdl");
			meshes = ObjectHelper.GetMeshes(model, dev);

			if(renderAssist == null) 
			{
				renderAssist = Mesh.Box(dev, 2f, 2f, 2f);
			}

			if(Material == null)
			{
				Material = new NJS_MATERIAL
				{
					DiffuseColor = Color.FromArgb(200, Color.Purple),
					SpecularColor = Color.Black,
					UseAlpha = true,
					DoubleSided = false,
					Exponent = 10,
					IgnoreSpecular = false,
					UseTexture = false
				};
			}
		}

		public override string Name { get { return "Air Spring"; } }
	}
}