﻿/*
 * Copyright (c) 2011 Stephen A. Pratt
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using System.Runtime.InteropServices;

namespace org.critterai.nav.rcn
{

    /// <summary>
    /// Polygon data for a polygon in a navigation mesh tile.
    /// </summary>
    /// <remarks>
    /// <p>Must be initialized before use.</p>
    /// <p>This data is provided for debug purposes.</p>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct NavmeshPoly
    {
        /// <summary>
        /// Index to first link in the linked list. 
        /// (Or <see cref="Navmesh.NullLink"/> if there is no link.)
        /// </summary>
        public uint firstLink;

        /// <summary>
        /// Indices to the polygon's vertices.
        /// </summary>
        /// <remarks>
        /// <p>Length: <see cref="Navmesh.MaxVertsPerPolygon"/>.</p>
        /// <p>The indices refer vertices in the polygon's 
        /// <see cref="NavmeshTileData">tile</see>.</p>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray
            , SizeConst = Navmesh.MaxVertsPerPolygon)]
	    public ushort[] indices;	                    

        /// <summary>
        /// Packed data representing neighbor polygon ids and flags for each 
        /// edge.
        /// </summary>
        /// <remarks>
        /// <p>Length: <see cref="Navmesh.MaxVertsPerPolygon"/>.</p>
        /// <p>Each entry represents data for the edge starting at the
        /// vertex of the same index.  E.g. The entry at index n represents
        /// the edge data for vertex[n] to vertex[n+1].</p>
        /// <p>A value of zero indicates the edge has no polygon connection.
        /// (It makes up the border of the navigation mesh.)</p>
        /// <p>The polygon id can be found as follows:
        /// (int)neighborPolyIds[n] & 0xff</p>
        /// <p>The edge is an external (portal) edge if the following test
        /// is TRUE: (neighborPolyIds[n] & Navmesh.ExternalLink) == 0</p>
        /// </remarks>
        [MarshalAs(UnmanagedType.ByValArray
            , SizeConst = Navmesh.MaxVertsPerPolygon)]
        public ushort[] neighborPolyIds;

        /// <summary>
        /// The polygon flags.
        /// </summary>
	    public ushort flags;

        /// <summary>
        /// The number of vertices in the polygon.
        /// </summary>
        /// <remarks>
        /// <p>The value will be between 3 and 
        /// <see cref="Navmesh.MaxVertsPerPolygon"/> inclusive for standard
        /// polygons, and 2 for off-mesh connections.</p>
        /// </remarks>
	    public byte vertexCount;

        /// <summary>
        /// A packed value.  See associated properties.
        /// </summary>
        private byte mAreaAndType;		
	
        /// <summary>
        /// The polygon's area id.
        /// </summary>
        public byte Area
        {
            get { return (byte)(mAreaAndType & 0x3f); }
        }

        /// <summary>
        /// The type of polygon.
        /// </summary>
        public NavmeshPolyType Type
        {
            get { return (NavmeshPolyType)(mAreaAndType >> 6); }
        }

        /// <summary>
        /// Initializes the structure before its first use.
        /// </summary>
        /// <remarks>
        /// Existing references are released and replaced.
        /// </remarks>
        public void Initialize()
        {
            indices = new ushort[Navmesh.MaxVertsPerPolygon];
            neighborPolyIds = new ushort[Navmesh.MaxVertsPerPolygon]; 
            flags = 0;
            vertexCount = 0;
            mAreaAndType = 0;
            firstLink = 0;
        }

        /// <summary>
        /// Rerturns an array of fully initialized structures.
        /// </summary>
        /// <param name="length">The length of the array. (>0)</param>
        /// <returns>An array of fully initialized structures.</returns>
        public static NavmeshPoly[] GetInitializedArray(int size)
        {
            NavmeshPoly[] result = new NavmeshPoly[size];
            for (int i = 0; i < result.Length; i++)
                result[i].Initialize();
            return result;
        }
    }
}
