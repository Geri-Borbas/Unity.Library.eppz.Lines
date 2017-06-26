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
	/// Submitting drawing in a `DirectLineRenderer` gets drawn for a single frame.
	/// Suitable for dynamic drawngs where underlying line model keep changing every
	/// frame.
	/// </summary>
	public class DirectLineRenderer : LineRendererBase
	{



		#region Events

			void Awake()
			{
				// Collect.
				LineRendererCamera.AddDirectRenderer(this);
			}

			public override void OnLineRendererCameraPostRender()
			{
				// if (this.isActiveAndEnabled == false) return; // Only if active
				OnDraw(); // Collect lines to the batch from subclasses
			}

			protected virtual void OnDraw()
			{
				/// <summary>
				/// Subclass template.
				/// Use `DrawLine`, `DrawRect`, `DrawCircle` and more to draw.
				/// </summary>
			}

		#endregion


		#region Batch lines

			protected override void DrawLine(Vector3 from, Vector3 to, Color color)
			{
				if (Application.isPlaying)
				{
					// Create and batch.
					Line line = new Line();
					line.from = from;
					line.to = to;
					line.color = color;

					// Collect directly.
					LineRendererCamera.shared.BatchLine(line);
				}
			}

		#endregion
	}
}