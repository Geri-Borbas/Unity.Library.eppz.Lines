//
// Copyright (c) 2017 Geri Borbás http://www.twitter.com/_eppz
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;


public class Lissajous : EPPZ.Lines.DirectLineRenderer
{


	float t = 0.0f;

	const float resolution = 0.01f;
	const float speed = 0.2f;


	protected override void OnDraw ()
	{
		// Draw Lissajous curve segments.
		Vector2 from = Function(t, 0.0f);
		for (float s = resolution; s < Mathf.PI * (2.0 + resolution); s += resolution)
		{
			Vector2 to = Function(t, s);

			// Direct draw.
			DrawLine(from, to, Color.white);

			from = to;
		}

		// Step.
		t += speed;
	}

	Vector2 Function(float t, float s)
	{
		// More at https://en.wikipedia.org/wiki/Lissajous_curve
		return new Vector2(
			Mathf.Sin (5 * s + t),
			Mathf.Sin (4 * s + t)
		);
	}
}
