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
using UnityEngine.Serialization;


namespace EPPZ.Lines
{


	/// <summary>
	/// Base class for two types of line renderers.
	/// Not meant for direct client useage.
	/// </summary>

#if EPPZ_UTILS
	using EPPZ.Utils;
	[ExecutionOrder (1100)]
#endif

	public class LineRendererBase : MonoBehaviour
	{


		// Preserve backward compatibility.
		[FormerlySerializedAs("debugMode")]
		[HideInInspector]
		public bool isActive = true;


		#region Events

			// Internally invoked by `LineRendererCamera.OnPostRender` (if any).
			public virtual void OnLineRendererCameraPostRender()
			{
				// Subclass template.
			}

		#endregion


		#region Batch lines

			protected virtual void DrawLine(Vector3 from, Vector3 to, Color color)
			{ 
				// Subclass template.
			}

		#endregion


		#region Drawing methods

			protected void DrawPoints(Vector2[] points, Color color, bool closed = true)
			{ DrawPointsWithTransform(points, color, null, closed); }

			protected void DrawPointsWithTransform(Vector2[] points, Color color, Transform transform_, bool closed = true)
			{
				int lastIndex = (closed) ? points.Length : points.Length - 1;
				for (int index = 0; index < lastIndex; index++)
				{
					Vector2 eachPoint = points[index];
					Vector2 eachNextPoint = (index < points.Length - 1) ? points[index + 1] : points[0];

					// Apply shape transform (if any).
					if (transform_ != null)
					{
						eachPoint = transform_.TransformPoint(eachPoint);
						eachNextPoint = transform_.TransformPoint(eachNextPoint);
					}

					// Draw.
					DrawLine(eachPoint, eachNextPoint, color);
				}
			}

			protected void DrawRect(Rect rect, Color color)
			{ DrawRectWithTransform(rect, color, null); }

			protected void DrawRectWithTransform(Rect rect, Color color, Transform transform_)
			{
				Vector2 leftTop = new Vector2(rect.xMin, rect.yMin);
				Vector2 rightTop = new Vector2(rect.xMax, rect.yMin);
				Vector2 rightBottom = new Vector2(rect.xMax, rect.yMax);
				Vector2 leftBottom = new Vector2(rect.xMin, rect.yMax);

				if (transform_ != null)
				{
					leftTop = transform_.TransformPoint(leftTop);
					rightTop = transform_.TransformPoint(rightTop);
					rightBottom = transform_.TransformPoint(rightBottom);
					leftBottom = transform_.TransformPoint(leftBottom);
				}

				DrawLine(
					leftTop,
					rightTop,
					color);

				DrawLine(
					rightTop,
					rightBottom,
					color);

				DrawLine(
					rightBottom,
					leftBottom,
					color);

				DrawLine(
					leftTop,
					leftBottom,
					color);
			}

			protected void DrawCircle(Vector2 center, float radius, int segments, Color color)
			{ DrawCircleWithTransform(center, radius, segments, color, null); }

			protected void DrawCircleWithTransform(Vector2 center, float radius, int segments, Color color, Transform transform_)
			{
				Vector2[] vertices = new Vector2[segments];

				// Compose a half circle (and mirrored) in normalized space (at 0,0).
				float angularStep = Mathf.PI * 2.0f / (float)segments;
				for (int index = 0; index < 1 + segments / 2; index++)
				{
					// Trigonometry.
					float angle = (float)index * angularStep;
					float x = Mathf.Sin(angle);
					float y = Mathf.Cos(angle);

					Vector2 vertex = new Vector2(x * radius, y * radius);
					Vector2 mirrored =  new Vector2(-x * radius, y * radius);

					// Save right, then left.
					vertices[index] = vertex;
					if (index > 0) vertices[segments-index] = mirrored;
				}

				// Draw around center.
				for (int index = 0; index < segments - 1; index++)
				{
					if (transform_ != null)
					{
						DrawLineWithTransform(
							center + vertices[index],
							center + vertices[index + 1],
							color,
							transform
						);
					}
					else
					{
						DrawLine(
							center + vertices[index],
							center + vertices[index + 1],
							color
						);
					}
				}

				// Last segment.
				if (transform_ != null)
				{
					DrawLineWithTransform(
						center + vertices[segments - 1],
						center + vertices[0],
						color,
						transform
					);
				}
				else
				{
					DrawLine(
						center + vertices[segments - 1],
						center + vertices[0],
						color
					);
				}
			}

			protected void DrawLineWithTransform(Vector2 from, Vector2 to, Color color, Transform transform_)
			{
				Vector2 from_ = transform_.TransformPoint(from);
				Vector2 to_ = transform_.TransformPoint(to);
				DrawLine (from_, to_, color);
			}

		#endregion


	}
}
