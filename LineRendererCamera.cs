//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace EPPZ.Lines
{


	/// <summary>
	/// Should be added to any camera that renders the scene.
	/// All the EPPZ.Lines.LineRenderer instances (collected
	/// on Awake) gonna be notified by this component after
	/// this camera finished rendering.
	/// </summary>

#if EPPZ_UTILS
	using EPPZ.Utils;
	[ExecutionOrder (1000)]
#endif

	public class LineRendererCamera : MonoBehaviour
	{


		// Singleton access.
		public static LineRendererCamera shared;

		// Camera.
		private Camera _camera;

		// Renderers.
		public enum UpdateMode { Update, LateUpdate };
		public UpdateMode update = UpdateMode.Update;
		public List<DirectLineRenderer> directLineRenderers = new List<DirectLineRenderer>();
		public List<CachedLineRenderer> cachedLineRenderers = new List<CachedLineRenderer>();
		private List<EPPZ.Lines.Line> lineBatch = new List<EPPZ.Lines.Line>(); 

		// Material for drawing.
		[Tooltip("Use material with vertex color for colors.")] public Material material;


		public static void AddDirectRenderer(DirectLineRenderer renderer)
		{ shared.directLineRenderers.Add(renderer); }

		public static void AddCachedRenderer(CachedLineRenderer renderer)
		{ shared.cachedLineRenderers.Add(renderer); }

		void Awake()
		{
			shared = this; 
			_camera = GetComponent<Camera>();
		}

		void OnPreRender()
		{ }

		void Update()
		{
			if (update == UpdateMode.Update)
			{
				BatchLines();
				DrawLines();
			}
		}

		void LateUpdate()
		{
			if (update == UpdateMode.LateUpdate)
			{
				BatchLines();
				DrawLines();
			}
		}

		void BatchLines()
		{
			// Flush.
			lineBatch.Clear();

			// Batch lines from direct renderers.
			foreach (EPPZ.Lines.DirectLineRenderer eachDirectLineRenderer in directLineRenderers)
			{
				if (eachDirectLineRenderer == null) continue;
				eachDirectLineRenderer.OnLineRendererCameraPostRender();
			}

			// Add up line collections from cached renderers.
			foreach (EPPZ.Lines.CachedLineRenderer eachCachedLineRenderer in cachedLineRenderers)
			{
				if (eachCachedLineRenderer == null) continue;
				lineBatch.AddRange(eachCachedLineRenderer.lines);
			}
		}

		void OnPostRender()
		{
			GL.PushMatrix();
			GL.LoadProjectionMatrix(_camera.projectionMatrix);
			DrawCall();
			GL.PopMatrix();
		}

		public void BatchLine(EPPZ.Lines.Line line)
		{ lineBatch.Add(line); }

		void DrawLines()
		{
			foreach (EPPZ.Lines.Line eachLine in lineBatch)
			{
				// Draw in Scene view.
				Debug.DrawLine(eachLine.from, eachLine.to, eachLine.color);
			}
		}

		void DrawCall()
		{
			// Assign vertex color material.
			material.SetPass(0); // Single draw call (set pass call)

			// Send vertices in GL_LINES Immediate Mode.
			GL.Begin(GL.LINES);
			foreach (EPPZ.Lines.Line eachLine in lineBatch)
			{
				GL.Color(eachLine.color);
				GL.Vertex(eachLine.from);
				GL.Vertex(eachLine.to);
			}
			GL.End();
		}
	}
}