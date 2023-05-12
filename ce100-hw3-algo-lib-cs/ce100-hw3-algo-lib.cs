using System.Collections;

namespace ce100_hw3_algo 
{
/// <summary>
/// The Shortest Route Class finds the shortest route between two locations in a complex city road network using the Bellman-Ford algorithm.
/// </summary>
public class Shortest_Route_Class {
  /// <summary>
  /// The Edge class stores edge information between two vertexes.
  /// </summary>
  public class Edge {
    /// <summary>
    /// Gets or sets the source vertex of the edge.
    /// </summary>
    public int SourceVertex {
      get;
      set;
    }
    /// <summary>
    /// Gets or sets the target vertex of the edge.
    /// </summary>
    public int DestinationVertex {
      get;
      set;
    }
    /// <summary>
    /// Gets or sets the length of the edge.
    /// </summary>
    public int Lenght {
      get;
      set;
    }
  }

  /// <summary>
  /// Retrieves the Edges list.
  /// </summary>
  public List<Edge> Edges {
    get
    {
      return edges;
    }
  }

  private int V, E;
  private List<Edge> edges;

  /// <summary>
  /// The constructor method of the Shortest_Route_Class class.
  /// </summary>
  /// <param name="v">Vertex count</param>
  /// <param name="e">Number of sides</param>
  public Shortest_Route_Class(int v, int e) {
    V = v;
    E = e;
    edges = new List<Edge>();
  }

  /// <summary>
  /// Adds edge.
  /// </summary>
  /// <param name="SourceVertex">Source vertex</param>
  /// <param name="DestinationVertex">Destination vertex</param>
  /// <param name="Lenght">Side length</param>
  public void AddEdge(int SourceVertex, int DestinationVertex, int Lenght) {
    edges.Add(new Edge { SourceVertex = SourceVertex, DestinationVertex = DestinationVertex, Lenght = Lenght });
  }

  /// <summary>
  /// Calculates the shortest route using the Bellman-Ford algorithm.
  /// </summary>
  /// <param name="SourceVertex">Initial vertex</param>
  /// <returns>An array containing the shortest routes of all vertexes</returns>
  /// <exception cref="InvalidOperationException"></exception>

  public int[] BellmanFord(int SourceVertex) {
    int[] distance = Enumerable.Repeat(int.MaxValue, V).ToArray();
    distance[SourceVertex] = 0;

    for (int i = 1; i < V; ++i) {
      edges.ForEach(edge => {
        if (distance[edge.SourceVertex] != int.MaxValue && distance[edge.SourceVertex] + edge.Lenght < distance[edge.DestinationVertex])
          distance[edge.DestinationVertex] = distance[edge.SourceVertex] + edge.Lenght;
      });
    }

    if (edges.Any(edge => distance[edge.SourceVertex] != int.MaxValue && distance[edge.SourceVertex] + edge.Lenght < distance[edge.DestinationVertex]))
      throw new InvalidOperationException("Graph contains negative Lenght cycle");

    return distance;
  }
}

/// <summary>
/// The File Compression Decompression Class compresses and decompresses the "mp3 extension music file" of the specified size with the huffman algorithm,
/// and these operations also read and write bytes.
/// </summary>
public class File_Compression_Decompression {
  /// <summary>
  /// A class for creating a Huffman Tree node.
  /// </summary>
  public class MinHNode {
    /// <summary>
    /// The frequency of the node.
    /// </summary>
    public int freq {
      get;
      set;
    }

    /// <summary>
    /// The character or item associated with the node.
    /// </summary>
    public char Items {
      get;
      set;
    }

    /// <summary>
    /// The left child of the node.
    /// </summary>
    public MinHNode left {
      get;
      set;
    }

    /// <summary>
    /// The right child of the node.
    /// </summary>
    public MinHNode right {
      get;
      set;
    }

    /// <summary>
    /// Recursively traverses the Huffman Tree to get the bit array representation of the specified character.
    /// </summary>
    /// <param name="items">The character to find the bit array representation of.</param>
    /// <param name="data">The list of bits representing the character.</param>
    /// <returns>The list of bits representing the character.</returns>
    public List<bool> Transition(char items, List<bool> data) {
      if (Items == items) {
        return data;
      } else if (left != null && left.Transition(items, new List<bool>(data) {
      false
    }) != null)
      return left.Transition(items, new List<bool>(data) {
        false
      });
      else if (right != null && right.Transition(items, new List<bool>(data) {
      true
    }) != null) {
        return right.Transition(items, new List<bool>(data) {
          true
        });
      } else {
        return null;
      }
    }
  }

  /// <summary>
  /// A class for creating a Huffman Tree.
  /// </summary>
  public class HuffmanTree_Class {
    private List<MinHNode> nodes = new List<MinHNode>();

    /// <summary>
    /// The root node of the Huffman Tree.
    /// </summary>
    public MinHNode Root {
      get;
      set;
    }

    /// <summary>
    /// A dictionary containing the frequency of each character in the input string.
    /// </summary>
    public Dictionary<char, int> Freq = new Dictionary<char, int>();

    /// <summary>
    /// Builds the Huffman Tree from the input string.
    /// </summary>
    /// <param name="source">The input string to build the Huffman Tree from.</param>
    public void Build(string source) {
      foreach (char c in source) {
        if (!Freq.ContainsKey(c)) {
          Freq.Add(c, 0);
        }

        Freq[c]++;
      }

      foreach (KeyValuePair<char, int> pair in Freq) {
        nodes.Add(new MinHNode() {
          Items = pair.Key,
          freq = pair.Value
        });
      }

      while (nodes.Count > 1) {
        List<MinHNode> orderedNodes = nodes.OrderBy(node => node.freq).ToList();
        List<MinHNode> taken = orderedNodes.Take(2).ToList();
        MinHNode parent = new MinHNode() {
          Items = '*',
          freq = taken[0].freq + taken[1].freq,
          left = taken[0],
          right = taken[1]
        };
        nodes.Remove(taken[0]);
        nodes.Remove(taken[1]);
        nodes.Add(parent);
        this.Root = nodes.FirstOrDefault();
      }
    }

    /// <summary>
    /// Encodes a string using a Huffman tree and returns the result as a BitArray.
    /// </summary>
    /// <param name="source">The string to encode.</param>
    /// <returns>A BitArray containing the encoded data.</returns>
    public BitArray Encode(string source) {
      List<bool> encSrc = new List<bool>();

      for (int i = 0; i < source.Length; i++) {
        List<bool> encSymbol = this.Root.Transition(source[i], new List<bool>());
        encSrc.AddRange(encSymbol);
      }

      BitArray bits = new BitArray(encSrc.ToArray());
      return bits;
    }


    /// <summary>
    /// Decodes a BitArray using a Huffman tree and returns the decoded string.
    /// </summary>
    /// <param name="bits">The BitArray to decode.</param>
    /// <returns>The decoded string.</returns>
    public string Decode(BitArray bits) {
      MinHNode currentN = this.Root;
      string decoded = "";

      foreach (bool bit in bits) {
        if (bit) {
          if (currentN.right != null) {
            currentN = currentN.right;
          }
        } else {
          if (currentN.left != null) {
            currentN = currentN.left;
          }
        }

        if (IsLeaf(currentN)) {
          decoded += currentN.Items;
          currentN = this.Root;
        }
      }

      return decoded;
    }


    /// <summary>
    /// Checks if a given node is a leaf node
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is a leaf node, false otherwise.</returns>
    public bool IsLeaf(MinHNode node) {
      return (node.left == null && node.right == null);
    }


  }

  /// <summary>
  /// A node in the minimum heap tree used to encode and decode music data.
  /// </summary>
  public class MinHNode_Music {
    /// <summary>
    /// The byte value of this node, representing a music symbol.
    /// </summary>
    public byte Items {
      get;
      set;
    }

    /// <summary>
    /// The frequency of the music symbol.
    /// </summary>
    public int Freq {
      get;
      set;
    }

    /// <summary>
    /// The left child node.
    /// </summary>
    public MinHNode_Music left {
      get;
      set;
    }

    /// <summary>
    /// The right child node.
    /// </summary>
    public MinHNode_Music right {
      get;
      set;
    }

    /// <summary>
    /// Recursively traverse the tree to generate a list of boolean values that represent the encoded bit stream of a given music symbol.
    /// </summary>
    /// <param name="items">The music symbol to encode.</param>
    /// <param name="data">The bit stream generated so far.</param>
    /// <returns>A list of boolean values that represent the encoded bit stream of the given music symbol.</returns>
    public List<bool> Transition_Music(byte? items, List<bool> data) {
      if (left == null && right == null) {
        if (Items == items) {
          return data;
        } else {
          return null;
        }
      } else {
        List<bool> Left = null;
        List<bool> Right = null;

        if (left != null) {
          List<bool> leftP = new List<bool>();
          leftP.AddRange(data);
          leftP.Add(false);
          Left = left.Transition_Music(items, leftP);
        }

        if (right != null) {
          List<bool> rightP = new List<bool>();
          rightP.AddRange(data);
          rightP.Add(true);
          Right = right.Transition_Music(items, rightP);
        }

        if (Left != null) {
          return Left;
        } else {
          return Right;
        }
      }
    }
  }


  /// <summary>
  /// HuffmanTree_Class_Music class for building Huffman tree and encoding/decoding data using the tree.
  /// </summary>
  public class HuffmanTree_Class_Music {
    /// <summary>
    /// List of MinHNode_Music objects representing each node in the Huffman tree.
    /// </summary>
    private List<MinHNode_Music> nodes = new List<MinHNode_Music>();

    /// <summary>
    /// Root node of the Huffman tree.
    /// </summary>
    public MinHNode_Music Root {
      get;
      set;
    }

    /// <summary>
    /// Dictionary that stores the frequency of each byte in the input data.
    /// </summary>
    public Dictionary<byte, int> Freq = new Dictionary<byte, int>();

    /// <summary>
    /// Builds a Huffman tree using the frequency of each byte in the input data.
    /// </summary>
    /// <param name="source">Byte array containing the input data.</param>
    public void Build(byte[] source) {
      for (int i = 0; i < source.Length; i++) {
        if (!Freq.ContainsKey(source[i])) {
          Freq.Add(source[i], 0);
        }

        Freq[source[i]]++;
      }

      foreach (KeyValuePair<byte, int> item in Freq) {
        nodes.Add(new MinHNode_Music() {
          Items = item.Key,
          Freq = item.Value
        });
      }

      while (nodes.Count > 1) {
        List<MinHNode_Music> orderedNodes = nodes.OrderBy(node => node.Freq).ToList<MinHNode_Music>();

        if (orderedNodes.Count >= 2) {
          List<MinHNode_Music> taken = orderedNodes.Take(2).ToList<MinHNode_Music>();
          MinHNode_Music parent = new MinHNode_Music() {
            Items = byte.MaxValue,
            Freq = taken[0].Freq + taken[1].Freq,
            left = taken[0],
            right = taken[1]
          };
          nodes.Remove(taken[0]);
          nodes.Remove(taken[1]);
          nodes.Add(parent);
        }

        this.Root = nodes.FirstOrDefault();
      }
    }

    /// <summary>
    /// Encodes the input data using the Huffman tree.
    /// </summary>
    /// <param name="source">Byte array containing the input data.</param>
    /// <returns>BitArray object containing the encoded data.</returns>
    public BitArray Encode(byte[] source) {
      List<bool> encSrc = new List<bool>();

      for (int i = 0; i < source.Length; i++) {
        List<bool> encSymbol = this.Root.Transition_Music(source[i], new List<bool>());
        encSrc.AddRange(encSymbol);
      }

      BitArray bits = new BitArray(encSrc.ToArray());
      return bits;
    }

    /// <summary>
    /// Decodes a given bit array using a Huffman tree.
    /// </summary>
    /// <param name="bits">The bit array to decode.</param>
    /// <returns>The decoded byte array.</returns>
    public byte[] Decode(BitArray bits) {
      MinHNode_Music currentN = this.Root;
      List<byte> decoded = new List<byte>();

      foreach (bool bit in bits) {
        if (bit) {
          if (currentN.right != null) {
            currentN = currentN.right;
          }
        } else {
          if (currentN.left != null) {
            currentN = currentN.left;
          }
        }

        if (IsLeaf(currentN)) {
          decoded.Add(currentN.Items);
          currentN = this.Root;
        }
      }

      return decoded.ToArray();
    }

    /// <summary>
    /// Checks if a given node is a leaf node.
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns>True if the node is a leaf node, false otherwise.</returns>
    public bool IsLeaf(MinHNode_Music node) {
      return (node.left == null && node.right == null);
    }

  }

  /// <summary>
  /// A static class containing helper methods for Huffman encoding and decoding.
  /// </summary>
  public class Huffman_Helper {
    /// <summary>
    /// Creates a Lorem Ipsum text of a specified length.
    /// </summary>
    /// <param name="length">The desired length of the text in characters.</param>
    /// <returns>A string containing the Lorem Ipsum text with the specified length.</returns>
    public static string CreatingLoremIpsum(long length) {
      string loremIpsum_Text =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor. Cras elementum ultrices diam. Maecenas ligula massa, varius a, semper congue, euismod non, mi.";
      int repeatC = (int)Math.Ceiling((double)length / loremIpsum_Text.Length);
      string loremIpsum = "";

      for (int i = 0; i < repeatC; i++) {
        loremIpsum += loremIpsum_Text;
      }

      loremIpsum = loremIpsum.Substring(0, (int)length);
      return loremIpsum;
    }

    /// <summary>
    /// Writes a BitArray to a binary file using a BinaryWriter.
    /// </summary>
    /// <param name="writer">The BinaryWriter to use for writing the BitArray.</param>
    /// <param name="bits">The BitArray to write.</param>
    public static void WriteBitArray(BinaryWriter writer, BitArray bits) {
      byte[] bytes = new byte[(bits.Length + 7) / 8];
      bits.CopyTo(bytes, 0);
      writer.Write(bytes);
    }

    /// <summary>
    /// Reads a BitArray from a binary file using a BinaryReader.
    /// </summary>
    /// <param name="reader">The BinaryReader to use for reading the BitArray.</param>
    /// <param name="byteCount">The number of bytes to read from the file.</param>
    /// <returns>The BitArray read from the file.</returns>
    public static BitArray ReadBitArray(BinaryReader reader, long byteCount) {
      List<bool> bits = new List<bool>();
      byte[] bytes = reader.ReadBytes((int)byteCount);

      foreach (byte b in bytes) {
        for (int i = 0; i < 8; i++) {
          bits.Add((b & (1 << i)) != 0);
        }
      }

      return new BitArray(bits.ToArray());
    }
  }

}

/// <summary>
    /// \brief A class that represents a pipeline system.
    /// </summary>
public class Pipeline_System
    {
        /// <summary>
        /// \brief A class that represents an edge in a pipeline system.
        /// </summary>
        public class Edge
        {
            /// <summary>
            /// The starting node of the edge.
            /// </summary>
            public int StartN
            {
                get;
                set;
            }
            /// <summary>
            /// The ending node of the edge.
            /// </summary>
            public int EndN
            {
                get;
                set;
            }
            /// <summary>
            /// The weight of the edge.
            /// </summary>
            public int Way
            {
                get;
                set;
            }
        }

        /// <summary>
        /// The edges in the pipeline system.
        /// </summary>
        private List<Edge> edges;

        /// <summary>
        /// The parent nodes of the edges.
        /// </summary>
        private int[] parents;

        /// <summary>
        /// \brief Initializes a new instance of the Pipeline_System class.
        /// </summary>
        /// <param name="edges"></param>
        public Pipeline_System(List<Edge> edges)
        {
            this.edges = edges.OrderBy(edge => edge.Way).ToList();
            parents = new int[edges.Count];

            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = i;
            }
        }

        /// <summary>
        /// \brief Implements the Kruskal's algorithm to find the minimum cost of a pipeline system.
        /// \return The minimum cost of the pipeline system.
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public int KruskalAlgo(List<Edge> edges)
        {
            int minimumcost = 0;

            foreach (Edge edge in edges)
            {
                int StartNParent = FindParent(edge.StartN);
                int EndNParent = FindParent(edge.EndN);

                if (StartNParent != EndNParent)
                {
                    minimumcost += edge.Way;
                    parents[StartNParent] = EndNParent;
                }
            }

            return minimumcost;
        }

        /// <summary>
        /// \brief Finds the parent node of a vertex.
        /// \return The parent node of the vertex.
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        private int FindParent(int vertex)
        {
            if (parents[vertex] != vertex)
            {
                parents[vertex] = FindParent(parents[vertex]);
            }

            return parents[vertex];
        }
    }

/// <summary>
    /// A class for generating a furniture assembly guide based on dependencies between different furniture items.
    /// </summary>
public class Furniture_Assembly_Guide
    {
        /// <summary>
        /// A class representing a furniture item.
        /// </summary>
        public class Furniture
        {
            /// <summary>
            /// The name of the furniture item.
            /// </summary>
            public string Name
            {
                get;
                set;
            }
            /// <summary>
            /// The ID of the furniture item.
            /// </summary>
            public int Id
            {
                get;
                set;
            }
            /// <summary>
            /// A list of IDs of furniture items that this item depends on.
            /// </summary>
            public List<int> Dependencies
            {
                get;
                set;
            }

            /// <summary>
            /// Creates a new instance of the Furniture class with the specified name and ID.
            /// </summary>
            /// <param name="name">The name of the furniture item.</param>
            /// <param name="id">The ID of the furniture item.</param>
            public Furniture(string name, int id)
            {
                Name = name;
                Id = id;
                Dependencies = new List<int>();
            }
        }

        /// <summary>
        /// A list of furniture items.
        /// </summary>
        private List<Furniture> Furnitures;

        /// <summary>
        /// Creates a new instance of the Furniture_Assembly_Guide class.
        /// </summary>
        public Furniture_Assembly_Guide()
        {
            Furnitures = new List<Furniture>();
        }

        /// <summary>
        /// Adds a list of furniture items to the assembly guide.
        /// </summary>
        /// <param name="Furniture">The list of furniture items to add.</param>
        public void AddFurniture(List<Furniture> Furniture)
        {
            Furnitures.AddRange(Furniture);
        }

        /// <summary>
        /// Performs a depth-first search starting from the specified ID and adds the visited nodes to the specified stack.
        /// </summary>
        /// <param name="id">The ID of the node to start the search from.</param>
        /// <param name="visited">A set of IDs of nodes that have already been visited.</param>
        /// <param name="stack">A stack to store the visited nodes in reverse order.</param>
        private void DFS(int id, HashSet<int> visited, Stack<int> stack)
        {
            visited.Add(id);

            foreach (int dependency in Furnitures[id - 1].Dependencies)
            {
                if (!visited.Contains(dependency))
                {
                    DFS(dependency, visited, stack);
                }
            }

            stack.Push(id);
        }

        /// <summary>
        /// Returns a list of furniture item IDs in the order that they should be assembled based on their dependencies.
        /// </summary>
        /// <returns>A list of furniture item IDs in the order that they should be assembled.</returns>
        public List<int> TopologicalSort()
        {
            HashSet<int> visited = new HashSet<int>();
            Stack<int> stack = new Stack<int>();

            foreach (Furniture Furniture in Furnitures)
            {
                if (!visited.Contains(Furniture.Id))
                {
                    DFS(Furniture.Id, visited, stack);
                }
            }

            return stack.Reverse().ToList();
        }

        /// <summary>
        /// Generates an assembly guide for the furniture items based on their dependencies.
        /// </summary>
        /// <returns>An array list of assembly steps for the furniture items.</returns>
        public ArrayList GetAssemblySteps()
        {
            List<Furniture> sortedFurnitures = Furnitures.OrderBy(Furniture => TopologicalSort().IndexOf(Furniture.Id)).ToList();
            ArrayList assemblySteps = new ArrayList();

            for (int i = 0; i < sortedFurnitures.Count; i++)
            {
                Furniture Furniture = sortedFurnitures[i];
                string dependencies = string.Join(", ", Furniture.Dependencies.Select(depId => Furnitures.First(depFurniture => depFurniture.Id == depId).Name));
                assemblySteps.Add($"{i + 1}. Attach {Furniture.Name}");
            }

            return assemblySteps;
        }
    }

}
