using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OBJParser
{
    interface IDoubleDatabase
    {
        public double this[int i] { get; }

        public int SetDouble(double value);
    }

    interface IOBJLineElement
    {
        public bool IsValidOBJ(string LineValue);

        public void ParseOBJ(string LineValue);

        public string PrintOBJ();
    }

    class Vertex3D : IOBJLineElement
    {
        private int lineNumber;
        private double x;
        private double y;
        private double z;

        public Vertex3D() : this(0, 0, 0)
        {
        }

        public Vertex3D(double X, double Y, double Z)
            : this(0, X, Y, Z)
        {
        }

        public Vertex3D(int LineNumber, double X, double Y, double Z)
        {
            lineNumber = LineNumber;
            x = X;
            y = Y;
            z = Z;
        }

        public Vertex3D(int X, int Y, int Z, IDoubleDatabase db)
        {
            x = db[X];
            y = db[Y];
            z = db[Z];
        }

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public bool IsValidOBJ(string LineValue)
        {
            return IsStringValidOBJ(LineValue);
        }

        static bool IsStringValidOBJ(string LineValue)
        {
            string[] line_contents = LineValue.Split(" ");
            bool isValid = (line_contents[0] == "v") && (line_contents.Length == 4);
            double throwaway = 0;
            foreach (string element in line_contents.Skip(1))
            {
                isValid = isValid && double.TryParse(element, out throwaway);
            }
            return isValid;
        }

        public void ParseOBJ(string LineValue)
        {
            string[] line_contents = LineValue.Split(" ");
            x = double.Parse(line_contents[1]);
            y = double.Parse(line_contents[2]);
            z = double.Parse(line_contents[3]);
        }

        private static Vertex3D ParseNewOBJ(string LineValue)
        {
            Vertex3D v = new Vertex3D();
            v.ParseOBJ(LineValue);
            return v;
        }

        public string PrintOBJ()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("v").Append(" ");
            builder.Append(x).Append(" ");
            builder.Append(y).Append(" ");
            builder.Append(z).Append(" ");
            return builder.ToString();
        }

        public static IEnumerable<Vertex3D> GetVerticesFromLines(string filename)
        {
            return from line in System.IO.File.ReadAllLines(filename)
                   where IsStringValidOBJ(line)
                   select ParseNewOBJ(line);
        }

        public double GetMagnitudeSquared()
        {
            return X * X + Y * Y + Z * Z;
        }
    }

    struct Vector3D
    {
        public double x;
        public double y;
        public double z;

        public Vector3D(double X, double Y, double Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
    }

    struct Vertex2D
    {
        public double x;
        public double y;

        public Vertex2D(double X, double Y)
        {
            x = X;
            y = Y;
        }
    }

    class FaceVertex
    {
        private int vertexIndex;
        private bool hasTextureVertex;
        private int textureVertexIndex;
        private bool hasNormal;
        private int normalIndex;

        public FaceVertex(int VertexIndex)
            : this(VertexIndex,false,0,false,0)
        {

        }

        public FaceVertex(int VertexIndex, int NormalIndex)
            : this(VertexIndex, false, 0, true, NormalIndex)
        {

        }

        public FaceVertex(int VertexIndex, int vertexIndex, int NormalIndex)
            : this(VertexIndex, true, vertexIndex, true, NormalIndex)
        {

        }

        public FaceVertex(int VertexIndex,
                    bool HasTextureVertex,
                    int TextureVertexIndex,
                    bool HasNormal,
                    int NormalIndex)
        {
            vertexIndex = VertexIndex;
            hasTextureVertex = HasTextureVertex;
            textureVertexIndex = TextureVertexIndex;
            hasNormal = HasNormal;
            normalIndex = NormalIndex;
        }

        public int VertexIndex
        {
            get { return vertexIndex; }
            set { vertexIndex = value; }
        }
        public bool HasTextureVertex
        {
            get { return hasTextureVertex; }
            set { hasTextureVertex = value; }
        }
        public int TextureVertex
        {
            get { return textureVertexIndex; }
            set { textureVertexIndex = value; }
        }
        public bool HasNormal
        {
            get { return hasNormal; }
            set { hasNormal = value; }
        }
        public int NormalIndex
        {
            get { return normalIndex; }
            set { NormalIndex = value; }
        }

    }

    struct OBJObject
    {
        public int index;
        public int position;
        public int lastPosition;
        public string name;

        public IEnumerable<Vertex3D> vertices;
        public IEnumerable<Vertex2D> textureVertices;
        public IEnumerable<Vector3D> normals;
        public IEnumerable<IEnumerable<FaceVertex>> faces;

        public OBJObject(int Index,
                         int Position,
                         int LastPosition,
                         string Name,
                         IEnumerable<Vertex3D> Vertices,
                         IEnumerable<Vertex2D> TextureVertices,
                         IEnumerable<Vector3D> Normals,
                         IEnumerable<IEnumerable<FaceVertex>> Faces)
        {
            index = Index;
            position = Position;
            lastPosition = LastPosition;
            name = Name;
            vertices = Vertices;
            textureVertices = TextureVertices;
            normals = Normals;
            faces = Faces;
        }

        public List<Vertex3D> CalculateConvexHull(List<Vertex3D> face)
        {
            return face;
        }

        public bool IsPointInCircumcircle(List<Vertex3D> Triangle, Vertex3D point)
        {
            //bool retVal = false;
            //Vertex3D Circumcenter;
            //if(Triangle.Count() == 3)
            //{
            //    OpenTK.Mathematics.Vector3d p0;
            //    OpenTK.Mathematics.Vector3d p1;
            //    OpenTK.Mathematics.Vector3d p2;
            //    var p1 = Triangle[1];
            //    var p2 = Triangle[2];

            //    var dA = Triangle[0].GetMagnitudeSquared();
            //    var dB = Triangle[1].GetMagnitudeSquared();
            //    var dC = Triangle[2].GetMagnitudeSquared();


                
            //}

            //var aux1 = (dA * (p2.Y - p1.Y) + dB * (p0.Y - p2.Y) + dC * (p1.Y - p0.Y));
            //var aux2 = -(dA * (p2.X - p1.X) + dB * (p0.X - p2.X) + dC * (p1.X - p0.X));
            //var div = (2 * (p0.X * (p2.Y - p1.Y) + p1.X * (p0.Y - p2.Y) + p2.X * (p1.Y - p0.Y)));


            //var d_squared = (point.X - Circumcenter.X) * (point.X - Circumcenter.X) +
            //    (point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y);
            return false; // d_squared < RadiusSquared;
        }

        public IEnumerable<IEnumerable<FaceVertex>> Triangulate(IEnumerable<FaceVertex> face, IEnumerable<Vertex3D> vertices)
        {
            List<FaceVertex> faceList = face.ToList();
            //List<Vertex3D> faceVertices = (from fv in face
            //                               where fv.VertexIndex < vertices.Count()
            //                               select vertices.ToList()[fv.VertexIndex]).ToList();

            List<IEnumerable<FaceVertex>> triangles = new List<IEnumerable<FaceVertex>>();

            if (face.Count() > 3)
            {
                //List<Vertex3D> convexHull = CalculateConvexHull(faceVertices);
                List<FaceVertex> first = face.ToList().GetRange(0, 3);
                triangles.Add(first);
                FaceVertex firstPoint = first[0];
                FaceVertex intermediatePoint = first[2];
                foreach(FaceVertex v in face.ToList().GetRange(3,face.Count() - 3))
                {
                    triangles.Add(new FaceVertex[]{ firstPoint, intermediatePoint, v });
                    intermediatePoint = v;
                }
            }

            return triangles;
        }
    }

    class Program
    {
        static void outputStdOBJ(string outfile, IEnumerable<OBJObject> objectList)
        {
            System.IO.FileStream outputfile = new System.IO.FileStream(outfile, System.IO.FileMode.Create);

            System.IO.StreamWriter writer = new System.IO.StreamWriter(outputfile);
            foreach (var o in objectList)
            {
                    writer.WriteLine("o {0}", o.name);
                    foreach (var f in o.faces)
                    {
                        writer.Write("f");
                        foreach (var e in f)
                        {
                            writer.Write(" {0}", e.VertexIndex);
                            if (e.HasTextureVertex || e.HasNormal)
                            {
                                writer.Write("/");
                                if (e.HasTextureVertex)
                                {
                                    writer.Write("{0}", e.TextureVertex);
                                }
                                if (e.HasNormal)
                                {
                                    writer.Write("/{0}", e.NormalIndex);
                                }
                            }
                        }
                        writer.WriteLine();
                    }
                    foreach (var v in o.vertices)
                    {
                        writer.WriteLine("v {0} {1} {2}", v.X, v.Y, v.Z);
                    }
                    foreach (var vt in o.textureVertices)
                    {
                        writer.WriteLine("vt {0} {1}", vt.x, vt.y);
                    }
                    foreach (var n in o.normals)
                    {
                        writer.WriteLine("vn {0} {1} {2}", n.x, n.y, n.z);
                    }
                }

            writer.Close();
            outputfile.Close();
        }

        static void outputOBJAsYAML(string outfile, IEnumerable<OBJObject> objectList)
        {
            System.IO.FileStream outputfile = new System.IO.FileStream(outfile, System.IO.FileMode.Create);

            System.IO.StreamWriter writer = new System.IO.StreamWriter(outputfile);

            var floatList = from floatEntry in (from list in
                                                    from obj in objectList
                                                    from vertex in obj.vertices
                                                    select new List<double>()
                                                        {
                                                            vertex.X,
                                                            vertex.Y,
                                                            vertex.Z
                                                        }
                                                from element in list
                                                select element).Concat(
                                                    from list in
                                                        from obj in objectList
                                                        from vertex in obj.textureVertices
                                                        select new List<double>()
                                                        {
                                                            vertex.x,
                                                            vertex.y
                                                        }
                                                    from element in list
                                                    select element).Concat(
                                                    from list in
                                                        from obj in objectList
                                                        from normal in obj.normals
                                                        select new List<double>()
                                                        {
                                                            normal.x,
                                                            normal.y,
                                                            normal.z
                                                        }
                                                    from element in list
                                                    select element)
                            select floatEntry;

            var uniquefloats = (from f in floatList
                                orderby (from v in floatList
                                         where v == f
                                         select v).Count() descending
                                select f).Distinct();

            Dictionary<double, int> floatDict
                        = new Dictionary<double, int>(uniquefloats.Zip(Enumerable.Range(1, floatList.Count()),
                                                                       (f, i) => new KeyValuePair<double, int>(f, i)));

            var ivertices = from o in objectList
                            from vertex in o.vertices
                            select new
                            {
                                ownerID = o.index,
                                x = floatDict[vertex.X],
                                y = floatDict[vertex.Y],
                                z = floatDict[vertex.Z]
                            };

            var itexturevertices = from o in objectList
                                   from vertex in o.textureVertices
                                   select new
                                   {
                                       ownerID = o.index,
                                       x = floatDict[vertex.x],
                                       y = floatDict[vertex.y]
                                   };

            var inormals = from o in objectList
                           from normal in o.normals
                           select new
                           {
                               ownerID = o.index,
                               x = floatDict[normal.x],
                               y = floatDict[normal.y],
                               z = floatDict[normal.z]
                           };

            writer.WriteLine("# OBJ SemiOptimized YAML file created by James Larsen for kicks");
            writer.WriteLine("Objects: ");
            foreach (var o in objectList)
            {
                writer.WriteLine("  Name: {0}", o.name);
                writer.WriteLine("    Faces:");
                foreach (var f in o.faces)
                {
                    writer.Write("      - [");
                    string delim = "";
                    foreach (var e in f)
                    {
                        writer.Write("{0}{1}", delim, e.VertexIndex);
                        if (e.HasNormal || e.HasTextureVertex)
                        {
                            writer.Write("/");
                            if (e.HasTextureVertex)
                            {
                                writer.Write("{0}", e.TextureVertex);
                            }
                            if (e.HasNormal)
                            {
                                writer.Write("/{0}", e.NormalIndex);
                            }
                        }
                        delim = ",";
                    }
                    writer.WriteLine("]");
                }
                writer.WriteLine("    Vertices:");
                foreach (var v in from v in ivertices
                                  where v.ownerID == o.index
                                  select v)
                {
                    writer.WriteLine("      - [{0},{1},{2}]", v.x, v.y, v.z);
                }
                writer.WriteLine("    TextureVertices:");
                foreach (var vt in from v in itexturevertices
                                   where v.ownerID == o.index
                                   select v)
                {
                    writer.WriteLine("      - [{0},{1}]", vt.x, vt.y);
                }
                foreach (var n in from v in inormals
                                  where v.ownerID == o.index
                                  select v)
                {
                    writer.WriteLine("      - [{0},{1},{2}]", n.x, n.y, n.z);
                }
            }
            writer.Write("Floats: [");
            string floatdelim = "";
            foreach (var f in uniquefloats)
            {
                writer.Write("{0}{1}", floatdelim, f);
                floatdelim = ",";
            }
            writer.WriteLine("]");

            writer.Close();
            outputfile.Close();
        }

        static void outputSlightlyOptimizedOBJ(string outfile, IEnumerable<OBJObject> objectList)
        {
            System.IO.FileStream outputfile = new System.IO.FileStream(outfile, System.IO.FileMode.Create);

            System.IO.StreamWriter writer = new System.IO.StreamWriter(outputfile);

            var floatList = from floatEntry in (from list in
                                                    from obj in objectList
                                                    from vertex in obj.vertices
                                                    select new List<double>()
                                                        {
                                                            vertex.X,
                                                            vertex.Y,
                                                            vertex.Z
                                                        }
                                                from element in list
                                                select element).Concat(
                                                    from list in
                                                        from obj in objectList
                                                        from vertex in obj.textureVertices
                                                        select new List<double>()
                                                        {
                                                            vertex.x,
                                                            vertex.y
                                                        }
                                                    from element in list
                                                    select element).Concat(
                                                    from list in
                                                        from obj in objectList
                                                        from normal in obj.normals
                                                        select new List<double>()
                                                        {
                                                            normal.x,
                                                            normal.y,
                                                            normal.z
                                                        }
                                                    from element in list
                                                    select element)
                            select floatEntry;

            var uniquefloats = (from f in floatList
                                orderby (from v in floatList
                                         where v == f
                                         select v).Count() descending
                                select f).Distinct();

            Dictionary<double, int> floatDict
                        = new Dictionary<double, int>(uniquefloats.Zip(Enumerable.Range(1, floatList.Count()),
                                                                       (f, i) => new KeyValuePair<double, int>(f, i)));

            var ivertices = from o in objectList
                            from vertex in o.vertices
                            select new
                            {
                                ownerID = o.index,
                                x = floatDict[vertex.X],
                                y = floatDict[vertex.Y],
                                z = floatDict[vertex.Z]
                            };

            var itexturevertices = from o in objectList
                                   from vertex in o.textureVertices
                                   select new
                                   {
                                       ownerID = o.index,
                                       x = floatDict[vertex.x],
                                       y = floatDict[vertex.y]
                                   };

            var inormals = from o in objectList
                           from normal in o.normals
                           select new
                           {
                               ownerID = o.index,
                               x = floatDict[normal.x],
                               y = floatDict[normal.y],
                               z = floatDict[normal.z]
                           };

            writer.WriteLine("# OBJ SemiOptimized file created by James Larsen for kicks");
            foreach (var o in objectList)
            {
                writer.WriteLine("o {0}", o.name);
                foreach (var f in o.faces)
                {
                    writer.Write("f");
                    foreach (var e in f)
                    {
                        writer.Write(" {0}", e.VertexIndex);
                        if (e.HasTextureVertex || e.HasNormal)
                        {
                            writer.Write("/");
                            if (e.HasTextureVertex)
                            {
                                writer.Write("{0}", e.TextureVertex);
                            }
                            if (e.HasNormal)
                            {
                                writer.Write("/{0}", e.NormalIndex);
                            }
                        }
                    }
                    writer.WriteLine();
                }
                foreach (var v in from v in ivertices
                                  where v.ownerID == o.index
                                  select v)
                {
                    writer.WriteLine("v {0} {1} {2}", v.x, v.y, v.z);
                }
                foreach (var vt in from v in itexturevertices
                                   where v.ownerID == o.index
                                   select v)
                {
                    writer.WriteLine("vt {0} {1}", vt.x, vt.y);
                }
                foreach (var n in from v in inormals
                                  where v.ownerID == o.index
                                  select v)
                {
                    writer.WriteLine("vn {0} {1} {2}", n.x, n.y, n.z);
                }
            }
            foreach (var f in uniquefloats)
            {
                writer.WriteLine("d {0}", f);
            }

            writer.Close();
            outputfile.Close();
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Need filename as first arg");
            }
            else
            {
                Console.WriteLine("Attempting to open file {0}", args[0]);
                var lines = System.IO.File.ReadLines(args[0]);
                var numberedLines = lines.Zip(Enumerable.Range(1, lines.Count()),
                                              (line, number) => new
                                              {
                                                  lineWords = line.Split(" "),
                                                  lineNumber = number
                                              }).ToList();

                var objects = from entry in numberedLines
                              where entry.lineWords.Length == 2
                                 && entry.lineWords[0] == "o"
                              select new
                              {
                                  lineNumber = entry.lineNumber,
                                  name = entry.lineWords[1]
                              };

                var numberedObjects = objects.Zip(Enumerable.Range(1, objects.Count()),
                                                  (obj, index) => new
                                                  {
                                                      name = obj.name,
                                                      lineNumber = obj.lineNumber,
                                                      i = index
                                                  });

                var rangedObjects = from element in from first in numberedObjects
                                                    from second in numberedObjects
                                                    where first.i + 1 == second.i
                                                       || (first.i == numberedObjects.Count()
                                                         && first.name == second.name)
                                                    select new
                                                    {
                                                        i = first.i,
                                                        position = first.lineNumber,
                                                        lastPosition = (first.lineNumber == second.lineNumber) ? lines.Count() : second.lineNumber,
                                                        name = first.name
                                                    }
                                    select new OBJObject(element.i,
                                                         element.position,
                                                         element.lastPosition,
                                                         element.name,
                                                         from entry in numberedLines.GetRange(element.position,element.lastPosition - element.position)
                                                         where entry.lineWords.Length == 4
                                                            && entry.lineWords[0] == "v"
                                                            && double.TryParse(entry.lineWords[1], out _)
                                                            && double.TryParse(entry.lineWords[2], out _)
                                                            && double.TryParse(entry.lineWords[3], out _)
                                                         select new Vertex3D(double.Parse(entry.lineWords[1]),
                                                                             double.Parse(entry.lineWords[2]),
                                                                             double.Parse(entry.lineWords[3])),

                                                         from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                         where entry.lineWords.Length == 3
                                                            && entry.lineWords[0] == "vt"
                                                            && double.TryParse(entry.lineWords[1], out _)
                                                            && double.TryParse(entry.lineWords[2], out _)
                                                         select new Vertex2D(double.Parse(entry.lineWords[1]),
                                                                             double.Parse(entry.lineWords[2])),

                                                         from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                         where entry.lineWords.Length == 4
                                                            && entry.lineWords[0] == "vn"
                                                            && double.TryParse(entry.lineWords[1], out _)
                                                            && double.TryParse(entry.lineWords[2], out _)
                                                            && double.TryParse(entry.lineWords[3], out _)
                                                         select new Vector3D(double.Parse(entry.lineWords[1]),
                                                                             double.Parse(entry.lineWords[2]),
                                                                             double.Parse(entry.lineWords[3])),
                                                         (from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                          where entry.lineWords.Length >= 4
                                                             && entry.lineWords[0] == "f"
                                                             && (from faceStringElement in entry.lineWords.Skip(1)
                                                                 where int.TryParse(faceStringElement, out _)
                                                                 select faceStringElement).Count() == entry.lineWords.Skip(1).Count()
                                                          select (from faceStringElement in entry.lineWords.Skip(1)
                                                                  select new FaceVertex(int.Parse(faceStringElement)))).Concat(

                                                          from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                          where entry.lineWords.Length >= 4
                                                            && entry.lineWords[0] == "f"
                                                            && (from faceStringElement in entry.lineWords.Skip(1)
                                                                where faceStringElement.Split("//").Length == 2
                                                                  && int.TryParse(faceStringElement.Split("//")[0], out _)
                                                                  && int.TryParse(faceStringElement.Split("//")[1], out _)
                                                                select faceStringElement).Count() == entry.lineWords.Skip(1).Count()
                                                          select (from faceStringElement in entry.lineWords.Skip(1)
                                                                  select new FaceVertex(int.Parse(faceStringElement.Split("//")[0]),
                                                                                  int.Parse(faceStringElement.Split("//")[1])))).Concat(
                                                          from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                          where entry.lineWords.Length >= 4
                                                              && entry.lineWords[0] == "f"
                                                              && (from faceStringElement in entry.lineWords.Skip(1)
                                                                  where faceStringElement.Split("/").Length == 2
                                                                    && int.TryParse(faceStringElement.Split("/")[0], out _)
                                                                    && int.TryParse(faceStringElement.Split("/")[1], out _)
                                                                  select faceStringElement).Count() == entry.lineWords.Skip(1).Count()
                                                            select (from faceStringElement in entry.lineWords.Skip(1)
                                                                    select new FaceVertex(int.Parse(faceStringElement.Split("/")[0]),
                                                                                    true,
                                                                                    int.Parse(faceStringElement.Split("/")[1]),
                                                                                    false,
                                                                                    0))).Concat(

                                                          from entry in numberedLines.GetRange(element.position, element.lastPosition - element.position)
                                                          where entry.lineWords.Length >= 4
                                                              && entry.lineWords[0] == "f"
                                                              && (from faceStringElement in entry.lineWords.Skip(1)
                                                                  where faceStringElement.Split("/").Length == 3
                                                                    && int.TryParse(faceStringElement.Split("/")[0], out _)
                                                                    && int.TryParse(faceStringElement.Split("/")[1], out _)
                                                                    && int.TryParse(faceStringElement.Split("/")[2], out _)
                                                                  select faceStringElement).Count() == entry.lineWords.Skip(1).Count()
                                                            select (from faceStringElement in entry.lineWords.Skip(1)
                                                                    select new FaceVertex(int.Parse(faceStringElement.Split("/")[0]),
                                                                                    int.Parse(faceStringElement.Split("/")[1]),
                                                                                    int.Parse(faceStringElement.Split("/")[2])))));

                var floatList = from floatEntry in (from list in
                                                        from obj in rangedObjects
                                                        from vertex in obj.vertices
                                                        select new List<double>()
                                                        {
                                                            vertex.X,
                                                            vertex.Y,
                                                            vertex.Z
                                                        }
                                                    from element in list
                                                    select element).Concat(
                                                    from list in
                                                        from obj in rangedObjects
                                                        from vertex in obj.textureVertices
                                                        select new List<double>()
                                                        {
                                                            vertex.x,
                                                            vertex.y
                                                        }
                                                    from element in list
                                                    select element).Concat(
                                                    from list in
                                                        from obj in rangedObjects
                                                        from normal in obj.normals
                                                        select new List<double>()
                                                        {
                                                            normal.x,
                                                            normal.y,
                                                            normal.z
                                                        }
                                                    from element in list
                                                    select element)
                                select floatEntry;

                var uniquefloats = (from f in floatList
                                    orderby (from v in floatList
                                             where v == f
                                             select v).Count() descending
                                    select f).Distinct();

                foreach (var o in rangedObjects)
                {
                    Console.WriteLine("Object name: {0}", o.name);
                    Console.WriteLine("Vertices: {0}", o.vertices.Count());
                    Console.WriteLine("Texture Vertices: {0}", o.textureVertices.Count());
                    Console.WriteLine("Normals: {0}", o.normals.Count());
                    Console.WriteLine("Faces: {0}", o.faces.Count());
                }
                Console.WriteLine("unique floats/all floats: {0}/{1}", uniquefloats.Count(), floatList.Count());

                if (args.Length >= 2)
                {
                    outputOBJAsYAML(args[1], rangedObjects);
                }
            }
        }
    }
}
