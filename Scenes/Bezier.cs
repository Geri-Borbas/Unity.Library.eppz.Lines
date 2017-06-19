//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;


public class Bezier : EPPZ.Lines.DirectLineRenderer
{


	public Transform a;
	public Transform b;
	public Transform c;
	[Range (1,50)] public int segments = 20;


	protected override void OnDraw()
	{
		// Increments.
		Vector3 increment_a_b = (b.position - a.position) / (float)segments;
		Vector3 increment_b_c = (c.position - b.position) / (float)segments;

		// Draw segments.
		for (int index = 0; index <= segments; index ++)
		{
			Vector2 from = a.position + (increment_a_b * index);
			Vector2 to = b.position + (increment_b_c * index);

			// Direct draw.
			DrawLine(from, to, Color.white);
		}
	}
}
