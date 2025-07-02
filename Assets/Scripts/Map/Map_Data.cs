using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Assimp.Configs;
using Assets;
using System.Linq;
using UnityDds;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Threading.Tasks;

public struct PieceData
{
	public Mesh visualMesh;
	public Mesh colliderMesh;
	public Material[] materials;
	public string script;
}


public class Map_Data : MonoBehaviour
{
	public GameObject Map;
	public GameObject HitArea;
	public GameObject SkyMap;
	public Assimp.AssimpImporter Importer = new Assimp.AssimpImporter();
	public List<Map_PieceBatch> pieceBatchList = new List<Map_PieceBatch>();
	public PieceData[] Pieces;
	public List<string> scripts = new List<string>();
	public string path = "E:\\Downloads\\WindomMapsDecoded\\map\\moon";
	public Material baseMat;
	public Material hitMat;
	public List<Vector3> point_0 = new List<Vector3>();
	public List<Vector3> point_1 = new List<Vector3>();
	public List<Vector3> point_2 = new List<Vector3>();
	public List<Vector3> point_3 = new List<Vector3>();
	public List<Vector3> point_4 = new List<Vector3>();
	public List<Vector3> point_5 = new List<Vector3>();
	public List<Vector3> point_6 = new List<Vector3>();
	public List<Vector3> point_7 = new List<Vector3>();
	public List<Vector3> point_8 = new List<Vector3>();
	public List<Vector3> point_9 = new List<Vector3>();
	public List<Vector3> point_rnd = new List<Vector3>();
    public CypherTranscoder transcoder;
	// Start is called before the first frame update
	void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void loadmpd()
    {
		mpd map = new mpd();
		map.load(Path.Combine(path, "map.mpd"));
		point_0 = new List<Vector3>();
		point_1 = new List<Vector3>();
		point_2 = new List<Vector3>();
		point_3 = new List<Vector3>();
		point_4 = new List<Vector3>();
		point_5 = new List<Vector3>();
		point_6 = new List<Vector3>();
		point_7 = new List<Vector3>();
		point_8 = new List<Vector3>();
		point_9 = new List<Vector3>();
		point_rnd = new List<Vector3>();


		Pieces = new PieceData[map.Pieces.Count];
		scripts = map.scripts;

		for (int j = 0; j < Pieces.Length; j++)
		{
			updatePiece(j, map.Pieces[j].visualMesh, map.Pieces[j].collisionMesh, map.Pieces[j].scriptText);
			if (map.Pieces[j].visualMesh != "")
			{
				
				Mesh visualM = new Mesh();
				Mesh colliderM = new Mesh();
				Material[] mats = new Material[0];
				bool isAlpha = false;
				try
				{
					if (j == 199)
						isAlpha = true;

					if (map.Pieces[j].visualMesh != "")
						ImportModel(Path.Combine(path, map.Pieces[j].visualMesh), ref visualM, ref mats, isAlpha);

					if (map.Pieces[j].visualMesh == map.Pieces[j].collisionMesh)
					{ colliderM = visualM; Debug.Log("Equals Visual"); }
					if (map.Pieces[j].collisionMesh != "")
					{ colliderM = ImportModel(Path.Combine(path, map.Pieces[j].collisionMesh)); Debug.Log("New Mesh Loaded"); }
				}
				catch
				{
					Debug.Log("Invalid Load: " + map.Pieces[j].visualMesh);
				}
				visualM.name = map.Pieces[j].visualMesh;
				if (colliderM != null)
					colliderM.name = map.Pieces[j].collisionMesh;

				Pieces[j].visualMesh = visualM;
				Pieces[j].colliderMesh = colliderM;
				Pieces[j].materials = mats;
				Pieces[j].script = map.Pieces[j].scriptText;

				
			}
		}

		ObjectData od;
		for (int x = 0; x < map.WorldGrid.GetLength(0); x++)
		{
			for (int y = 0; y < map.WorldGrid.GetLength(1); y++)
		{
			mpd_WorldGrid wg2 = map.WorldGrid[x,y];
			foreach (mpd_Object obj in wg2.objects)
			{

				if (Pieces[obj.pieceID].visualMesh != null)
				{
					if (Pieces[obj.pieceID].visualMesh.name.Contains("point"))
					{
							
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_0"))
								point_0.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_1"))
								point_1.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_2"))
								point_2.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_3"))
								point_3.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_4"))
								point_4.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_5"))
								point_5.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_6"))
								point_6.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_7"))
								point_7.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_8"))
								point_8.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_9"))
								point_9.Add(Utils.GetPosition(obj.transform));
							if (Pieces[obj.pieceID].visualMesh.name.Contains("point_rnd"))
								point_rnd.Add(Utils.GetPosition(obj.transform));
						}
					else
					{
						GameObject go = new GameObject(Pieces[obj.pieceID].visualMesh.name);
						go.layer = 6;
						od = go.AddComponent<ObjectData>();
						od.ModelID = obj.pieceID;
						od.scriptID = obj.scriptIndex;
						go.transform.SetParent(Map.transform);
						go.transform.localPosition = Utils.GetPosition(obj.transform);
						go.transform.localRotation = Utils.GetRotation(obj.transform);
						go.transform.localScale = Utils.GetScale(obj.transform);
						od.data = this;
						od.build();
					}

				}

			}
		}
		}

        HitArea = new GameObject("Hit.x");
        od = HitArea.AddComponent<ObjectData>();
        od.ModelID = 199;
        od.scriptID = -1;
        HitArea.transform.localPosition = new Vector3(1500, 0, 1500);
        od.data = this;
        od.build();
		HitArea.AddComponent<MeshCollider>().sharedMesh = Pieces[199].visualMesh;


		SkyMap = new GameObject("Sky.x");
		od = SkyMap.AddComponent<ObjectData>();
		od.ModelID = 198;
		od.scriptID = -1;
		SkyMap.transform.localPosition = new Vector3(1500, 0, 1500);
		SkyMap.transform.localScale = Vector3.one * 3000;
		od.data = this;
		od.build();

	}

	public void buildMapPart()
    {

    }

	public void save()
    {
		mpd map = new mpd();
		map.scripts = scripts;
		List<mpd_Piece> mpdPieces = new List<mpd_Piece>();
		for (int i = 0; i < Pieces.Length; i++)
        {
			mpd_Piece mpdPiece = new mpd_Piece();
			if (Pieces[i].visualMesh != null)
			{
				mpdPiece.visualMesh = Pieces[i].visualMesh.name;
				if (Pieces[i].colliderMesh != null)
					mpdPiece.collisionMesh = Pieces[i].colliderMesh.name;
				mpdPiece.scriptText = Pieces[i].script;
			}
			mpdPieces.Add(mpdPiece);
        }
		map.Pieces = mpdPieces;
		Transform[] transforms = Map.GetComponentsInChildren<Transform>();
		for (int i = 1; i < transforms.Length; i++)
        {
			Matrix4x4 m = Matrix4x4.TRS(transforms[i].position,transforms[i].rotation,transforms[i].localScale);
			ObjectData od = transforms[i].GetComponent<ObjectData>();
			if (od != null)
				map.addWorldObject(transforms[i].position,m,od.ModelID,od.scriptID);
        }
		map.save(Path.Combine(path, "map.mpd"));
	}

    public void updatePiece(int index, string visualMesh, string collisionMesh, string scriptText)
    {
		if (visualMesh != "")
		{

			Mesh visualM = new Mesh();
			Mesh colliderM = new Mesh();
			Material[] mats = new Material[0];
			bool isAlpha = false;
			try
			{
				if (index == 199)
					isAlpha = true;

				if (visualMesh != "")
					ImportModel(Path.Combine(path, visualMesh), ref visualM, ref mats, isAlpha);

				if (visualMesh == collisionMesh)
				{ colliderM = visualM; }
				if (collisionMesh != "")
				{ colliderM = ImportModel(Path.Combine(path, collisionMesh));  }
			}
			catch
			{
				Debug.Log("Invalid Load: " + visualMesh);
			}
			visualM.name = visualMesh;
			if (colliderM != null)
				colliderM.name = collisionMesh;

			Pieces[index].visualMesh = visualM;
			Pieces[index].colliderMesh = colliderM;
			if (index == 199)
            {
				for (int i = 0; i < mats.Length; i++)
                {
					Texture tex = mats[i].mainTexture;
					mats[i] = new Material(hitMat.shader);
					mats[i].mainTexture = tex;
                }
            }
			Pieces[index].materials = mats;
			Pieces[index].script = scriptText;
		}
	}
    public void build()
    {

    	for (int i = 0; i < pieceBatchList.Count; i++)
    	{
    		if (pieceBatchList[i] != null)
    		{
    			//Debug.Log("[build] ビルドを開始します");
    			pieceBatchList[i].destroyGameObjects();
    			pieceBatchList[i].generateGameObjects();
    		}
    	}
    }








    Mesh ImportModel(string file)
    {
        //Debug.Log($"[ImportModel(Mesh)] モデルのインポートを開始します: {file}");
        if (File.Exists(file))
        {
            try
            {
				string Modelpath = Path.GetDirectoryName(file);
                Debug.Log($"[ImportModel(Mesh)] モデルパス: {Modelpath}");
				byte[] data = transcoder.Transcode(file);
				if (data == null)
				{
					Debug.Log($"ファイルの変換に失敗しました: {file}。結果がnullです。");
					return null;
				}
				if (data.Length == 0)
				{
					Debug.Log($"ファイル {file} の変換データが空です。");
					return null;
				}
				if (data.Length > 0)
				{
					string data1 = System.Text.Encoding.GetEncoding("utf-8").GetString(data);
					data1 = XfileStringConverter(data1);
					byte[] data3 = System.Text.Encoding.GetEncoding("utf-8").GetBytes(data1);
					MemoryStream ms = new MemoryStream(data3);
					Assimp.Scene scen = null;
					try
					{
						scen = Importer.ImportFileFromStream(ms, Helper.PostProcessStepflags, "x");
					}
					catch (System.Exception e)
					{
						Debug.Log($"暗号化されたモデル {file} のインポート中にエラーが発生しました: {e.Message}。Assimpがファイルを読み込めませんでした。");
					}
					if (scen == null)
					{
						Debug.LogError("Assimpがファイルを読み込めませんでした: " + file);
						return null;
					}
					if (scen.Meshes.Count() == 0) {
						Debug.LogError("メッシュが存在しません: " + file);
						return null;
					}

				
					//var scen = Importer.ImportFile(file, Helper.PostProcessStepflags);
					//Debug.Log($"[ImportModel(Mesh)] シーンのインポートに成功しました: {file}");
					Mesh mesh = new Mesh();
					var combineInstances = scen.Meshes
						.Select(x => x.ToUnityMesh())
						.Where(unityMesh => unityMesh != null)
						.Select(unityMesh => new CombineInstance {
							mesh = unityMesh,
							transform = scen.RootNode.Transform.ToUnityMatrix()
						})
						.ToArray();

					if (combineInstances.Length == 0) {
						Debug.LogError($"No valid meshes to combine in file: {file}");
						return null;
					}

					mesh.CombineMeshes(combineInstances, false);
					//Debug.Log($"[ImportModel(Mesh)] メッシュの結合に成功しました: {file}");
					//Debug.Log($"[ImportModel(Mesh)] モデルのインポートが完了しました: {file}");
					return mesh;
				}
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ImportModel(Mesh)] モデルのインポート中にエラーが発生しました: {file} エラー: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError($"[ImportModel(Mesh)] ファイルが存在しません: {file}");
        }
		//Debug.Log($"[ImportModel(Mesh)] モデルのインポートが完了しました: {file}");
        return null;
    }
    public string XfileStringConverter(string data)
    {
        if (!data.Trim().EndsWith("}"))
        {
            //Debug.Log("文字化けを確認しました");
            int lastBraceIndex = data.LastIndexOf('}');
            if (lastBraceIndex != -1)
            {
                data = data.Substring(0, lastBraceIndex + 1); // 最後の波括弧を残す
            }
            data += "}"; // 新たに波括弧
            //Debug.Log("文字化けを対応しました。");
        }
        // FrameTransformMatrixの部分を探す正規表現        
        string pattern = @"FrameTransformMatrix\s*{([^}]*)}";
        MatchCollection matches = Regex.Matches(data, pattern, RegexOptions.Singleline);

        if (matches.Count >= 2) // 2回目のマッチが存在するか確認
        {
            
            string matrixContent = matches[1].Groups[1].Value;
            Debug.Log("マッチしました:" + matrixContent);
            // 4行目の数値を処理
            string[] lines = matrixContent.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            Debug.Log("要素数は" + lines.Length);
            if (lines.Length >= 16) // 4行目の数値があるか確認
            {
                Debug.Log("4行目を確認しました。");
                for (int i = 13; i < 16; i++) // 4行目の数値（3つ）を処理
                {
                    lines[i] = lines[i].TrimStart('-'); // マイナス符号を取り除く
                    Debug.Log("符号排除処理をしました");
                }

                // 新しい内容を生成
                string newMatrixContent = string.Join(",", lines);
                string output = data.Replace(matrixContent, newMatrixContent);
                Debug.Log($"書き換えました: {output.Substring(output.Length - 10)}");
                return output;
            }
            else
            {
                Debug.Log("4行目の数値が見つかりませんでした。");
            }
        }
        else
        {
            Debug.Log("FrameTransformMatrixが見つかりませんでした。");
        }



        return data;
    }

	void ImportModel(string file, ref Mesh meshes, ref Material[] materiales, bool isAlpha = false)
    {
        try
        {
            string Modelpath = Path.GetDirectoryName(file);
            byte[] data = transcoder.Transcode(file);
            if (data == null)
            {
                Debug.LogError($"[ImportModel] data==null: {file}");
                return;
            }
            if (data.Length == 0)
            {
                Debug.Log($"ファイル {file} の変換データが空です。");
                return;
            }
            if (data.Length > 0)
            {
                string data1 = System.Text.Encoding.GetEncoding("utf-8").GetString(data);
                data1 = XfileStringConverter(data1);
                byte[] data3 = System.Text.Encoding.GetEncoding("utf-8").GetBytes(data1);
                MemoryStream ms = new MemoryStream(data3);
                Assimp.Scene scen = null;
                try
                {
                    scen = Importer.ImportFileFromStream(ms, Helper.PostProcessStepflags, "x");
                }
                catch (System.Exception e)
                {
                	Debug.Log($"暗号化されたモデル {file} のインポート中にエラーが発生しました: {e.Message}。Assimpがファイルを読み込めませんでした。");
                }
                if (scen == null)
                {
                    Debug.LogWarning($"ストリームからの暗号化されたモデルのインポートに失敗しました: {file}。Assimpがファイルを読み込めませんでした。");
                    return;
                }
                if (scen.Meshes.Count() == 0) {
                    Debug.LogError("メッシュが存在しません: " + file);
                    return;
                }



                Mesh mesh = new Mesh();
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                
                try
                {
                    mesh.CombineMeshes(scen.Meshes.Select(x => {
                        var transform = scen.RootNode.Transform;
                        // 変換行列から負の値をチェックし、必要に応じて調整
                        if (transform.A1 < 0 || transform.A2 < 0 || transform.A3 < 0 ||
                            transform.B1 < 0 || transform.B2 < 0 || transform.B3 < 0 ||
                            transform.C1 < 0 || transform.C2 < 0 || transform.C3 < 0)
                        {
                            // 負の値が含まれている場合は単位行列を使用
                            transform = Assimp.Matrix4x4.Identity;
                        }

                        return new CombineInstance()
                        {
                            mesh = x.ToUnityMesh(),
                            transform = transform.ToUnityMatrix()
                        };
                    }).ToArray(), false);
                }
                catch (System.Exception e)
                {
                    Debug.Log($"ファイル {file} のメッシュ結合に失敗しました: {e.Message}");
                    return;
                }
                
                Material[] materials = new Material[scen.Meshes.Length];

                for (int index = 0; index < materials.Length; index++)
                {
                    var mat = new Material(Shader.Find("Standard"));
                    if (mat.shader == null) {
                        Debug.LogError("Standardシェーダーが見つかりません。");
                        // 必要なら代替シェーダーを指定
                    }

                    if (scen.Meshes[index].MaterialIndex < scen.Materials.Length)
                    {
                        if (scen.Materials[scen.Meshes[index].MaterialIndex] != null)
                        {
							if (isAlpha)
                            	mat.CopyPropertiesFromMaterial(baseMat);
                            mat.name = scen.Materials[scen.Meshes[index].MaterialIndex].Name;
                            var textures = scen.Materials[scen.Meshes[index].MaterialIndex].GetAllTextures();
                            var color = scen.Materials[scen.Meshes[index].MaterialIndex].ColorDiffuse;
                            mat.color = new Color(color.R, color.G, color.B, color.A);
                            mat.SetFloat("_Glossiness", scen.Materials[scen.Meshes[index].MaterialIndex].ShininessStrength);

                            // シェーダーが設定されていない場合、デフォルトのシェーダーを割り当てる
                            if (string.IsNullOrEmpty(mat.shader.name) || mat.shader == null)
                            {
                                Debug.LogWarning($"マテリアル {mat.name} のシェーダーが設定されていません。デフォルトのシェーダーを割り当てます。");
                                mat.shader = Shader.Find("Standard"); // デフォルトのシェーダーを設定
                            }

                            if (textures.Length > 0 && File.Exists(Path.Combine(Modelpath, textures[0].FilePath)))
                            {
                                try
                                {
                                    mat.mainTexture = Helper.LoadTextureEncrypted(Path.Combine(Modelpath, textures[0].FilePath), ref transcoder);
                                }
                                catch (System.Exception e)
                                {
                                    Debug.LogWarning($"ファイル {file} のテクスチャ読み込みに失敗しました: {e.Message}");
                                }
                            }
                            else
                            {
                                //Debug.Log($"マテリアルインデックス {scen.Meshes[index].MaterialIndex} にはテクスチャが設定されていません: {file}");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"メッシュ {index} の無効なマテリアルインデックス: {scen.Meshes[index].MaterialIndex}");
                    }

                    materials[index] = mat;
                }
                meshes = mesh;
                //part.AddComponent<MeshCollider>().sharedMesh = mesh; 
                materiales = materials;
            }
            else
            {
                Debug.LogWarning($"ファイル {file} の復号化に失敗しました。Assimpがファイルを読み込めませんでした。");
                return;
            }
        }
        catch (System.Exception e)
        {
            //Debug.Log($"暗号化されたモデル {file} の処理中にエラーが発生しました: {e.Message}。Assimpがファイルを読み込めませんでした。");
        }
    }

	Texture2D LoadDDS(string file)
    {
		byte[] data = File.ReadAllBytes(file);
		Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
		texture.LoadRawTextureData(data);
		texture.Apply();
		return texture;
    }
}
