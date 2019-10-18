﻿using Assimp;
using SharpDX;
using SonicRetro.SAModel.Direct3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace SonicRetro.SAModel.SAEditorCommon.Import
{
	public static class AssimpStuff
	{
		#region Export
		private class CachedPoly
		{
			public List<PolyChunk> Polys { get; private set; }
			public int Index { get; private set; }

			public CachedPoly(List<PolyChunk> polys, int index)
			{
				Polys = polys;
				Index = index;
			}
		}

		internal class WeightData
		{
			public int Index { get; private set; }
			public Vector3 Position { get; private set; }
			public Vector3 Normal { get; private set; }
			public float Weight { get; private set; }

			public WeightData(int index, Vector3 position, Vector3 normal, float weight)
			{
				Index = index;
				Position = position;
				Normal = normal;
				Weight = weight;
			}
		}

		static NJS_MATERIAL MaterialBuffer = new NJS_MATERIAL { UseTexture = true };
		static VertexData[] VertexBuffer = new VertexData[32768];
		static List<WeightData>[] WeightBuffer = new List<WeightData>[32768];
		static readonly CachedPoly[] PolyCache = new CachedPoly[255];
		static List<string> NodeNames;
		static List<Matrix> NodeTransforms;

		public static Node AssimpExportWeighted(this NJS_OBJECT obj, Scene scene, Matrix parentMatrix, string[] texInfo = null, Node parent = null)
		{
			NodeNames = new List<string>();
			NodeTransforms = new List<Matrix>();
			int mdlindex = -1;
			return AssimpExportWeighted(obj, scene, parentMatrix, texInfo, parent, ref mdlindex);
		}

		private static Node AssimpExportWeighted(this NJS_OBJECT obj, Scene scene, Matrix parentMatrix, string[] texInfo, Node parent, ref int mdlindex)
		{
			mdlindex++;
			string nodename = $"n{mdlindex:000}_{obj.Name}";
			Node node;
			if (parent == null)
				node = new Node(nodename);
			else
			{
				node = new Node(nodename, parent);
				parent.Children.Add(node);
			}
			NodeNames.Add(nodename);

			Matrix nodeTransform = Matrix.Identity;

			nodeTransform *= Matrix.Scaling(obj.Scale.X, obj.Scale.Y, obj.Scale.Z);

			float rotX = ((obj.Rotation.X) * (2 * (float)Math.PI) / 65536.0f);
			float rotY = ((obj.Rotation.Y) * (2 * (float)Math.PI) / 65536.0f);
			float rotZ = ((obj.Rotation.Z) * (2 * (float)Math.PI) / 65536.0f);
			Matrix rotation = Matrix.RotationX(rotX) *
					   Matrix.RotationY(rotY) *
					   Matrix.RotationZ(rotZ);
			nodeTransform *= rotation;

			nodeTransform *= Matrix.Translation(obj.Position.X, obj.Position.Y, obj.Position.Z);

			Matrix nodeWorldTransform = nodeTransform * parentMatrix;
			NodeTransforms.Add(nodeWorldTransform);
			Matrix nodeWorldTransformInv = Matrix.Invert(nodeWorldTransform);
			node.Transform = nodeTransform.ToAssimp();//nodeTransform;

			node.Name = nodename;
			int startMeshIndex = scene.MeshCount;
			if (obj.Attach != null)
			{
				ChunkAttach attach = (ChunkAttach)obj.Attach;
				if (attach.Vertex != null)
				{
					foreach (VertexChunk chunk in attach.Vertex)
					{
						if (chunk.HasWeight)
						{
							for (int i = 0; i < chunk.VertexCount; i++)
							{
								var weightByte = chunk.NinjaFlags[i] >> 16;
								var weight = weightByte / 255f;
								var origpos = chunk.Vertices[i].ToVector3();
								var position = (Vector3.TransformCoordinate(origpos, nodeWorldTransform) * weight).ToVertex();
								var orignor = Vector3.Up;
								Vertex normal = null;
								if (chunk.Normals.Count > 0)
								{
									orignor = chunk.Normals[i].ToVector3();
									normal = (Vector3.TransformNormal(orignor, nodeWorldTransform) * weight).ToVertex();
								}

								// Store vertex in cache
								var vertexId = chunk.NinjaFlags[i] & 0x0000FFFF;
								var vertexCacheId = (int)(chunk.IndexOffset + vertexId);

								if (chunk.WeightStatus == WeightStatus.Start)
								{
									// Add new vertex to cache
									VertexBuffer[vertexCacheId] = new VertexData(position, normal);
									WeightBuffer[vertexCacheId] = new List<WeightData>
								{
									new WeightData(mdlindex, origpos, orignor, weight)
								};
									if (chunk.Diffuse.Count > 0)
										VertexBuffer[vertexCacheId].Color = chunk.Diffuse[i];
								}
								else
								{
									// Update cached vertex
									var cacheVertex = VertexBuffer[vertexCacheId];
									cacheVertex.Position += position;
									cacheVertex.Normal += normal;
									if (chunk.Diffuse.Count > 0)
										cacheVertex.Color = chunk.Diffuse[i];
									VertexBuffer[vertexCacheId] = cacheVertex;
									WeightBuffer[vertexCacheId].Add(new WeightData(mdlindex, origpos, orignor, weight));
								}
							}
						}
						else
							for (int i = 0; i < chunk.VertexCount; i++)
							{
								var origpos = chunk.Vertices[i].ToVector3();
								var position = Vector3.TransformCoordinate(origpos, nodeWorldTransform).ToVertex();
								var orignor = Vector3.Up;
								Vertex normal = null;
								if (chunk.Normals.Count > 0)
								{
									orignor = chunk.Normals[i].ToVector3();
									normal = Vector3.TransformNormal(orignor, nodeWorldTransform).ToVertex();
								}
								VertexBuffer[i + chunk.IndexOffset] = new VertexData(position, normal);
								if (chunk.Diffuse.Count > 0)
									VertexBuffer[i + chunk.IndexOffset].Color = chunk.Diffuse[i];
								WeightBuffer[i + chunk.IndexOffset] = null;
								WeightBuffer[i + chunk.IndexOffset] = new List<WeightData>
							{
								new WeightData(mdlindex, origpos, orignor, 1)
							};
							}
					}
				}
				List<MeshInfo> result = new List<MeshInfo>();
				List<List<WeightData>> weights = new List<List<WeightData>>();
				if (attach.Poly != null)
					result = ProcessPolyList(attach.Poly, 0, weights);
				if (result.Count > 0)
				{
					int nameMeshIndex = 0;
					foreach (MeshInfo meshInfo in result)
					{
						Assimp.Mesh mesh = new Assimp.Mesh($"{attach.Name}_mesh_{nameMeshIndex}");

						NJS_MATERIAL cur_mat = meshInfo.Material;
						Material materoial = new Material() { Name = $"{attach.Name}_material_{nameMeshIndex++}" }; ;
						materoial.ColorDiffuse = new Color4D(cur_mat.DiffuseColor.R, cur_mat.DiffuseColor.G, cur_mat.DiffuseColor.B, cur_mat.DiffuseColor.A);
						if (cur_mat.UseTexture && texInfo != null)
						{
							string texPath = Path.GetFileName(texInfo[cur_mat.TextureID]);
							TextureWrapMode wrapU = TextureWrapMode.Wrap;
							TextureWrapMode wrapV = TextureWrapMode.Wrap;
							if (cur_mat.ClampU)
								wrapU = TextureWrapMode.Clamp;
							else if (cur_mat.FlipU)
								wrapU = TextureWrapMode.Mirror;

							if (cur_mat.ClampV)
								wrapV = TextureWrapMode.Clamp;
							else if (cur_mat.FlipV)
								wrapV = TextureWrapMode.Mirror;

							TextureSlot tex = new TextureSlot(texPath, TextureType.Diffuse, 0,
								TextureMapping.FromUV, 0, 1.0f, TextureOperation.Add,
								wrapU, wrapV, 0); //wrapmode and shit add here
							materoial.AddMaterialTexture(ref tex);
						}
						int matIndex = scene.MaterialCount;
						scene.Materials.Add(materoial);
						mesh.MaterialIndex = matIndex;

						mesh.PrimitiveType = PrimitiveType.Triangle;
						ushort[] tris = meshInfo.ToTriangles();
						List<List<WeightData>> vertexWeights = new List<List<WeightData>>(tris.Length);
						for (int i = 0; i < tris.Length; i += 3)
						{
							Face face = new Face();
							face.Indices.AddRange(new int[] { mesh.Vertices.Count + 2, mesh.Vertices.Count + 1, mesh.Vertices.Count });
							for (int j = 0; j < 3; j++)
							{
								mesh.Vertices.Add(Vector3.TransformCoordinate(meshInfo.Vertices[tris[i + j]].Position.ToVector3(), nodeWorldTransformInv).ToAssimp());
								mesh.Normals.Add(Vector3.TransformNormal(meshInfo.Vertices[tris[i + j]].Normal.ToVector3(), nodeWorldTransformInv).ToAssimp());
								if (meshInfo.Vertices[tris[i + j]].Color.HasValue)
									mesh.VertexColorChannels[0].Add(new Color4D(meshInfo.Vertices[tris[i + j]].Color.Value.R, meshInfo.Vertices[tris[i + j]].Color.Value.G, meshInfo.Vertices[tris[i + j]].Color.Value.B, meshInfo.Vertices[tris[i + j]].Color.Value.A));
								if (meshInfo.Vertices[tris[i + j]].UV != null)
									mesh.TextureCoordinateChannels[0].Add(new Vector3D(meshInfo.Vertices[tris[i + j]].UV.U, meshInfo.Vertices[tris[i + j]].UV.V, 1.0f));
								vertexWeights.Add(weights[tris[i + j]]);
							}
							mesh.Faces.Add(face);
						}

						// Convert vertex weights
						var aiBoneMap = new Dictionary<int, Bone>();
						for (int i = 0; i < vertexWeights.Count; i++)
						{
							for (int j = 0; j < vertexWeights[i].Count; j++)
							{
								var vertexWeight = vertexWeights[i][j];

								if (!aiBoneMap.TryGetValue(vertexWeight.Index, out var aiBone))
								{
									aiBone = aiBoneMap[vertexWeight.Index] = new Bone
									{
										Name = NodeNames[vertexWeight.Index]
									};

									// Offset matrix: difference between world transform of weighted bone node and the world transform of the mesh's parent node
									var offsetMatrix = Matrix.Invert(NodeTransforms[vertexWeight.Index] * nodeWorldTransformInv);
									aiBone.OffsetMatrix = offsetMatrix.ToAssimp();
								}

								// Assimps way of storing weights is not very efficient
								aiBone.VertexWeights.Add(new VertexWeight(i, vertexWeight.Weight));
							}
						}

						mesh.Bones.AddRange(aiBoneMap.Values);
						scene.Meshes.Add(mesh);
					}
					int endMeshIndex = scene.MeshCount;
					for (int i = startMeshIndex; i < endMeshIndex; i++)
					{
						//node.MeshIndices.Add(i);
						Node meshChildNode = new Node($"meshnode_{i}");
						meshChildNode.Transform = nodeWorldTransform.ToAssimp();
						scene.RootNode.Children.Add(meshChildNode);
						meshChildNode.MeshIndices.Add(i);
					}
				}
			}
			if (obj.Children != null)
			{
				foreach (NJS_OBJECT child in obj.Children)
					child.AssimpExportWeighted(scene, nodeWorldTransform, texInfo, node, ref mdlindex);
			}
			return node;
		}

		private static List<MeshInfo> ProcessPolyList(List<PolyChunk> strips, int start, List<List<WeightData>> weights)
		{
			List<MeshInfo> result = new List<MeshInfo>();
			for (int i = start; i < strips.Count; i++)
			{
				PolyChunk chunk = strips[i];
				switch (chunk.Type)
				{
					case ChunkType.Bits_BlendAlpha:
						{
							PolyChunkBitsBlendAlpha c2 = (PolyChunkBitsBlendAlpha)chunk;
							MaterialBuffer.SourceAlpha = c2.SourceAlpha;
							MaterialBuffer.DestinationAlpha = c2.DestinationAlpha;
						}
						break;
					case ChunkType.Bits_MipmapDAdjust:
						break;
					case ChunkType.Bits_SpecularExponent:
						MaterialBuffer.Exponent = ((PolyChunkBitsSpecularExponent)chunk).SpecularExponent;
						break;
					case ChunkType.Bits_CachePolygonList:
						byte cachenum = ((PolyChunkBitsCachePolygonList)chunk).List;
						PolyCache[cachenum] = new CachedPoly(strips, i + 1);
						return result;
					case ChunkType.Bits_DrawPolygonList:
						cachenum = ((PolyChunkBitsDrawPolygonList)chunk).List;
						CachedPoly cached = PolyCache[cachenum];
						result.AddRange(ProcessPolyList(cached.Polys, cached.Index, weights));
						break;
					case ChunkType.Tiny_TextureID:
					case ChunkType.Tiny_TextureID2:
						{
							PolyChunkTinyTextureID c2 = (PolyChunkTinyTextureID)chunk;
							MaterialBuffer.ClampU = c2.ClampU;
							MaterialBuffer.ClampV = c2.ClampV;
							MaterialBuffer.FilterMode = c2.FilterMode;
							MaterialBuffer.FlipU = c2.FlipU;
							MaterialBuffer.FlipV = c2.FlipV;
							MaterialBuffer.SuperSample = c2.SuperSample;
							MaterialBuffer.TextureID = c2.TextureID;
						}
						break;
					case ChunkType.Material_Diffuse:
					case ChunkType.Material_Ambient:
					case ChunkType.Material_DiffuseAmbient:
					case ChunkType.Material_Specular:
					case ChunkType.Material_DiffuseSpecular:
					case ChunkType.Material_AmbientSpecular:
					case ChunkType.Material_DiffuseAmbientSpecular:
					case ChunkType.Material_Diffuse2:
					case ChunkType.Material_Ambient2:
					case ChunkType.Material_DiffuseAmbient2:
					case ChunkType.Material_Specular2:
					case ChunkType.Material_DiffuseSpecular2:
					case ChunkType.Material_AmbientSpecular2:
					case ChunkType.Material_DiffuseAmbientSpecular2:
						{
							PolyChunkMaterial c2 = (PolyChunkMaterial)chunk;
							MaterialBuffer.SourceAlpha = c2.SourceAlpha;
							MaterialBuffer.DestinationAlpha = c2.DestinationAlpha;
							if (c2.Diffuse.HasValue)
								MaterialBuffer.DiffuseColor = c2.Diffuse.Value;
							if (c2.Specular.HasValue)
							{
								MaterialBuffer.SpecularColor = c2.Specular.Value;
								MaterialBuffer.Exponent = c2.SpecularExponent;
							}
						}
						break;
					case ChunkType.Strip_Strip:
					case ChunkType.Strip_StripUVN:
					case ChunkType.Strip_StripUVH:
					case ChunkType.Strip_StripNormal:
					case ChunkType.Strip_StripUVNNormal:
					case ChunkType.Strip_StripUVHNormal:
					case ChunkType.Strip_StripColor:
					case ChunkType.Strip_StripUVNColor:
					case ChunkType.Strip_StripUVHColor:
					case ChunkType.Strip_Strip2:
					case ChunkType.Strip_StripUVN2:
					case ChunkType.Strip_StripUVH2:
						{
							PolyChunkStrip c2 = (PolyChunkStrip)chunk;
							MaterialBuffer.DoubleSided = c2.DoubleSide;
							MaterialBuffer.EnvironmentMap = c2.EnvironmentMapping;
							MaterialBuffer.FlatShading = c2.FlatShading;
							MaterialBuffer.IgnoreLighting = c2.IgnoreLight;
							MaterialBuffer.IgnoreSpecular = c2.IgnoreSpecular;
							MaterialBuffer.UseAlpha = c2.UseAlpha;
							bool hasVColor = false;
							switch (chunk.Type)
							{
								case ChunkType.Strip_StripColor:
								case ChunkType.Strip_StripUVNColor:
								case ChunkType.Strip_StripUVHColor:
									hasVColor = true;
									break;
							}
							bool hasUV = false;
							switch (chunk.Type)
							{
								case ChunkType.Strip_StripUVN:
								case ChunkType.Strip_StripUVH:
								case ChunkType.Strip_StripUVNColor:
								case ChunkType.Strip_StripUVHColor:
								case ChunkType.Strip_StripUVN2:
								case ChunkType.Strip_StripUVH2:
									hasUV = true;
									break;
							}
							List<Poly> polys = new List<Poly>();
							List<VertexData> verts = new List<VertexData>();
							foreach (PolyChunkStrip.Strip strip in c2.Strips)
							{
								Strip str = new Strip(strip.Indexes.Length, strip.Reversed);
								for (int k = 0; k < strip.Indexes.Length; k++)
								{
									str.Indexes[k] = (ushort)verts.Count;
									verts.Add(new VertexData(
										VertexBuffer[strip.Indexes[k]].Position,
										VertexBuffer[strip.Indexes[k]].Normal,
										hasVColor ? (System.Drawing.Color?)strip.VColors[k] : VertexBuffer[strip.Indexes[k]].Color,
										hasUV ? strip.UVs[k] : null));
									weights.Add(WeightBuffer[strip.Indexes[k]]);
								}
								polys.Add(str);
							}
							result.Add(new MeshInfo(MaterialBuffer, polys.ToArray(), verts.ToArray(), hasUV, hasVColor));
							MaterialBuffer = new NJS_MATERIAL(MaterialBuffer);
						}
						break;
				}
			}
			return result;
		}
		#endregion

		#region Import
		static class VertexCacheManager
		{
			struct CacheEntry : IComparable<CacheEntry>
			{
				public int start;
				public int length;
				public int handle;

				public int end => start + length;

				int IComparable<CacheEntry>.CompareTo(CacheEntry other)
				{
					return start.CompareTo(other.start);
				}
			}

			static readonly SortedSet<CacheEntry> entries = new SortedSet<CacheEntry>();
			static int handle = 0;

			public static void Clear()
			{
				entries.Clear();
				handle = 0;
			}

			public static (int start, int handle) Reserve(int length)
			{
				int st = 0;
				foreach (var en in entries)
					if (en.start < st + length)
						st = en.end;
					else
						break;
				if (st + length > short.MaxValue)
					throw new Exception("No space in cache to reserve that many vertices!");
				CacheEntry entry = new CacheEntry
				{
					start = st,
					length = length,
					handle = handle++
				};
				entries.Add(entry);
				return (st, entry.handle);
			}

			public static void Release(int handle)
			{
				foreach (var en in entries)
					if (en.handle == handle)
					{
						entries.Remove(en);
						return;
					}
				throw new ArgumentException($"No cache entry with handle '{handle}' was found.", "handle");
			}
		}

		static Dictionary<string, int> nodeIndexForSort = new Dictionary<string, int>();

		private static void FillNodeIndexForSort(Scene scene, Node aiNode, ref int mdlindex)
		{
			mdlindex++;
			nodeIndexForSort.Add(aiNode.Name, mdlindex);
			foreach (Node child in aiNode.Children)
				FillNodeIndexForSort(scene, child, ref mdlindex);
		}

		class MeshData
		{
			public string FirstNode { get; set; }
			public Dictionary<string, List<VertexChunk>> Vertex { get; set; }
			public string LastNode { get; set; }
			public List<PolyChunk> Poly { get; set; }
			public int VertexCount { get; set; }
			public BoundingSphere Bounds { get; set; }
			public int CacheHandle { get; set; }

			public MeshData(int vcount)
			{
				VertexCount = vcount;
				Vertex = new Dictionary<string, List<VertexChunk>>();
				Poly = new List<PolyChunk>();
			}
		}

		class VertWeight
		{
			public readonly string name;
			public readonly float weight;

			public VertWeight(string name, float weight)
			{
				this.name = name;
				this.weight = weight;
			}
		}

		private static WeightStatus GetWeightStatus(string name, List<VertWeight> weights)
		{
			int i = weights.Select((w, ind) => (ind, w)).First(a => a.w.name == name).ind;
			if (i == 0)
				return WeightStatus.Start;
			else if (i == weights.Count - 1)
				return WeightStatus.End;
			else
				return WeightStatus.Middle;
		}

		static readonly NvTriStripDotNet.NvStripifier nvStripifier = new NvTriStripDotNet.NvStripifier() { StitchStrips = false, UseRestart = false };
		private static MeshData ProcessMesh(Assimp.Mesh aiMesh, Scene scene, string[] texInfo = null)
		{
			MeshData result = new MeshData(aiMesh.VertexCount);
			result.Bounds = SharpDX.BoundingSphere.FromPoints(aiMesh.Vertices.Select(a => a.ToSharpDX()).ToArray()).ToSAModel();
			var weights = new List<List<VertWeight>>(aiMesh.VertexCount);
			for (int i = 0; i < aiMesh.VertexCount; i++)
				weights.Add(new List<VertWeight>());
			List<string> sortedbones = new List<string>();
			var matrices = new Dictionary<string, Matrix>();
			foreach (var bone in aiMesh.Bones.Where(a => a.HasVertexWeights).OrderBy(a => nodeIndexForSort[a.Name]))
			{
				sortedbones.Add(bone.Name);
				foreach (var weight in bone.VertexWeights)
					weights[weight.VertexID].Add(new VertWeight(bone.Name, weight.Weight));
				matrices[bone.Name] = bone.OffsetMatrix.ToSharpDX();
			}
			result.FirstNode = sortedbones.First();
			string lastbone = sortedbones.Last();
			result.LastNode = lastbone;
			for (int i = 0; i < aiMesh.VertexCount; i++)
				if (weights[i].Count == 0)
					weights[i].Add(new VertWeight(lastbone, 1));
			List<(int orig, List<VertWeight> weights)> tmpweights = new List<(int orig, List<VertWeight> weights)>(aiMesh.VertexCount);
			for (int i = 0; i < weights.Count; i++)
				tmpweights.Add((i, weights[i]));
			tmpweights.Sort((a, b) =>
			{
				for (int i = 0; i < Math.Min(a.weights.Count, b.weights.Count); i++)
				{
					var c = nodeIndexForSort[a.weights[i].name].CompareTo(nodeIndexForSort[b.weights[i].name]);
					if (c != 0)
						return c;
				}
				return a.weights.Count.CompareTo(b.weights.Count);
			});
			Dictionary<int, int> vertmap = new Dictionary<int, int>(aiMesh.VertexCount);
			for (int i = 0; i < weights.Count; i++)
			{
				vertmap[tmpweights[i].orig] = i;
				weights[i] = tmpweights[i].weights;
			}
			foreach (var bonename in sortedbones)
			{
				List<VertexChunk> chunks = new List<VertexChunk>();
				var vertinds = weights.Select((weightlist, vertind) => (vertind, weightlist)).Where(a => a.weightlist.Any(b => b.name == bonename));
				VertexChunk vc = null;
				int nextind = -1;
				foreach (var (vertind, weightlist) in vertinds)
				{
					if (vertind != nextind || GetWeightStatus(bonename, weightlist) != vc.WeightStatus)
					{
						vc = new VertexChunk(ChunkType.Vertex_VertexNormalNinjaFlags) { IndexOffset = (ushort)vertind, WeightStatus = GetWeightStatus(bonename, weightlist) };
						chunks.Add(vc);
					}
					vc.Vertices.Add(Vector3.TransformCoordinate(aiMesh.Vertices[vertind].ToSharpDX(), matrices[bonename]).ToVertex());
					vc.Normals.Add(Vector3.TransformNormal(aiMesh.HasNormals ? aiMesh.Normals[vertind].ToSharpDX() : Vector3.Up, matrices[bonename]).ToVertex());
					vc.NinjaFlags.Add((uint)(((byte)(weightlist.First(a => a.name == bonename).weight * 255.0f) << 16) | (vertind - vc.IndexOffset)));
					nextind = vertind + 1;
				}
				foreach (var cnk in chunks)
				{
					cnk.VertexCount = (ushort)cnk.Vertices.Count;
					cnk.Size = (ushort)(cnk.VertexCount * 7 + 1);
				}

				result.Vertex.Add(bonename, chunks);
			}
			bool hasUV = aiMesh.HasTextureCoords(0);
			List<PolyChunkStrip.Strip> polys = new List<PolyChunkStrip.Strip>();
			List<ushort> tris = new List<ushort>();
			Dictionary<ushort, Vector3D> uvmap = new Dictionary<ushort, Vector3D>();
			foreach (Face aiFace in aiMesh.Faces)
				for (int i = 0; i < 3; i++)
				{
					ushort ind = (ushort)vertmap[aiFace.Indices[i]];
					if (hasUV)
						uvmap[ind] = aiMesh.TextureCoordinateChannels[0][aiFace.Indices[i]];
					tris.Add(ind);
				}

			nvStripifier.GenerateStrips(tris.ToArray(), out var primitiveGroups);
			foreach (NvTriStripDotNet.PrimitiveGroup grp in primitiveGroups)
			{
				var stripIndices = new ushort[grp.Indices.Length];
				List<UV> stripuv = new List<UV>();
				for (var j = 0; j < grp.Indices.Length; j++)
				{
					var vertexIndex = grp.Indices[j];
					stripIndices[j] = vertexIndex;
					if (hasUV)
						stripuv.Add(new UV() { U = uvmap[vertexIndex].X, V = 1.0f - uvmap[vertexIndex].Y });
				}

				polys.Add(new PolyChunkStrip.Strip(false, stripIndices, hasUV ? stripuv.ToArray() : null, null));
			}

			//material stuff
			Assimp.Material currentAiMat = scene.Materials[aiMesh.MaterialIndex];
			if (currentAiMat != null)
			{
				//output mat first then texID, thats how the official exporter worked
				PolyChunkMaterial material = new PolyChunkMaterial();
				result.Poly.Add(material);
				if (currentAiMat.HasTextureDiffuse)
				{
					if (texInfo != null)
					{
						PolyChunkTinyTextureID tinyTexId = new PolyChunkTinyTextureID();
						int texId = 0;
						for (int j = 0; j < texInfo.Length; j++)
							if (texInfo[j] == Path.GetFileNameWithoutExtension(currentAiMat.TextureDiffuse.FilePath))
								texId = j;
						tinyTexId.TextureID = (ushort)texId;
						result.Poly.Add(tinyTexId);
					}
				}
				else if (texInfo != null)
				{
					PolyChunkTinyTextureID tinyTexId = new PolyChunkTinyTextureID();
					int texId = 0;
					for (int j = 0; j < texInfo.Length; j++)
						if (texInfo[j].ToLower() == currentAiMat.Name.ToLower())
							texId = j;
					tinyTexId.TextureID = (ushort)texId;
					result.Poly.Add(tinyTexId);
				}
			}

			PolyChunkStrip strip;
			if (hasUV)
				strip = new PolyChunkStrip(ChunkType.Strip_StripUVN);
			else
				strip = new PolyChunkStrip(ChunkType.Strip_Strip);

			strip.Strips.AddRange(polys);
			result.Poly.Add(strip);
			return result;
		}

		public static NJS_OBJECT AssimpImportWeighted(Scene scene, string[] texInfo = null)
		{
			VertexCacheManager.Clear();
			nodeIndexForSort.Clear();

			//get node indices for sorting
			int mdlindex = -1;
			FillNodeIndexForSort(scene, scene.RootNode,ref mdlindex);
			List<MeshData> meshdata = new List<MeshData>();
			foreach (var mesh in scene.Meshes)
				meshdata.Add(ProcessMesh(mesh, scene, texInfo));
			mdlindex = -1;
			return AssimpImportWeighted(scene.RootNode.Children[0], scene, meshdata, scene.RootNode, ref mdlindex);
		}

		private static NJS_OBJECT AssimpImportWeighted(Node aiNode, Scene scene, List<MeshData> meshdata, Node parent, ref int mdlindex)
		{
			NJS_OBJECT obj = new NJS_OBJECT();
			obj.Name = aiNode.Name;
			Vector3D translation;
			Vector3D scaling;
			Assimp.Quaternion rotation;
			if (parent != null)
			{
				Matrix4x4 invParentTransform = parent.Transform;
				invParentTransform.Inverse();
				Matrix4x4 localTransform = aiNode.Transform * invParentTransform;
				localTransform.Decompose(out scaling, out rotation, out translation);
			}
			else
				aiNode.Transform.Decompose(out scaling, out rotation, out translation);
			Vector3D rotationConverted = rotation.ToEulerAngles();
			obj.Position = new Vertex(translation.X, translation.Y, translation.Z);
			//Rotation = new Rotation(0, 0, 0);
			obj.Rotation = new Rotation(Rotation.DegToBAMS(rotationConverted.X), Rotation.DegToBAMS(rotationConverted.Y), Rotation.DegToBAMS(rotationConverted.Z));
			obj.Scale = new Vertex(scaling.X, scaling.Y, scaling.Z);

			if (meshdata.Any(a => a.Vertex.ContainsKey(aiNode.Name)))
			{
				ChunkAttach attach = new ChunkAttach(true, meshdata.Any(a => a.LastNode == aiNode.Name));
				obj.Attach = attach;
				foreach (var mesh in meshdata.Where(a => a.Vertex.ContainsKey(aiNode.Name)))
				{
					if (mesh.FirstNode == aiNode.Name)
					{
						(int vstart, int handle) = VertexCacheManager.Reserve(mesh.VertexCount);
						mesh.CacheHandle = handle;
						foreach (var vc in mesh.Vertex.Values.SelectMany(a => a))
							vc.IndexOffset += (ushort)vstart;
						foreach (var str in mesh.Poly.OfType<PolyChunkStrip>().SelectMany(a => a.Strips))
							for (int i = 0; i < str.Indexes.Length; i++)
								str.Indexes[i] += (ushort)vstart;
					}
					attach.Vertex.AddRange(mesh.Vertex[aiNode.Name]);
					if (mesh.LastNode == aiNode.Name)
					{
						attach.Poly.AddRange(mesh.Poly);
						attach.Bounds = mesh.Bounds;
						VertexCacheManager.Release(mesh.CacheHandle);
					}
				}
			}
			foreach (Node child in aiNode.Children)
			{
				obj.AddChild(AssimpImportWeighted(child, scene, meshdata, aiNode, ref mdlindex));
			}
			return obj;
		}
		#endregion

		public static Vector3D ToAssimp(this Vector3 v) => new Vector3D(v.X, v.Y, v.Z);

		public static Vector3 ToSharpDX(this Vector3D v) => new Vector3(v.X, v.Y, v.Z);

		public static Matrix4x4 ToAssimp(this Matrix m) => new Matrix4x4(m.M11, m.M21, m.M31, m.M41, m.M12, m.M22, m.M32, m.M42, m.M13, m.M23, m.M33, m.M43, m.M14, m.M24, m.M34, m.M44);

		public static Matrix ToSharpDX(this Matrix4x4 m) => new Matrix(m.A1, m.B1, m.C1, m.D1, m.A2, m.B2, m.C2, m.D2, m.A3, m.B3, m.C3, m.D3, m.A4, m.B4, m.C4, m.D4);
	}
}
