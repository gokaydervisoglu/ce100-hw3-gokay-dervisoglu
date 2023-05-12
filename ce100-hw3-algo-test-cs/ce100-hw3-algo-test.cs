using ce100_hw3_algo;
using System.Collections;
using static ce100_hw3_algo.Furniture_Assembly_Guide;

namespace ce100_hw3_algo_test {
/// <summary>
/// Tests the shortest route distance calculation using the Bellman-Ford algorithm.
/// </summary>
public class Shortest_Route_Test_Class {

  [Fact]
  public void Shortest_Route_Distance_Test() {
    //Test
    // Arrange
    Shortest_Route_Class graph = new Shortest_Route_Class(5, 8);
    graph.AddEdge(0, 1, 6);
    graph.AddEdge(0, 3, 7);
    graph.AddEdge(1, 2, 5);
    graph.AddEdge(1, 3, 8);
    graph.AddEdge(1, 4, -4);
    graph.AddEdge(2, 1, -2);
    graph.AddEdge(3, 2, -3);
    graph.AddEdge(3, 4, 9);
    graph.AddEdge(4, 0, 2);
    graph.AddEdge(4, 2, 7);
    // Act
    int[] distances = graph.BellmanFord(0);
    // Assert
    Assert.Equal(new int[] { 0, 2, 4, 7, -2 }, distances);
  }

  [Fact]
  public void Shortest_Route_ShouldThrowException_Test() {
    // Arrange
    Shortest_Route_Class graph = new Shortest_Route_Class(5, 8);
    graph.AddEdge(0, 1, 6);
    graph.AddEdge(0, 3, 7);
    graph.AddEdge(1, 2, 5);
    graph.AddEdge(1, 3, 8);
    graph.AddEdge(1, 4, -4);
    graph.AddEdge(2, 1, -2);
    graph.AddEdge(3, 2, -3);
    graph.AddEdge(3, 4, 9);
    graph.AddEdge(4, 0, 2);
    graph.AddEdge(4, 2, -7);
    // Act and Assert
    Assert.Throws<InvalidOperationException>(() => graph.BellmanFord(0));
  }

  [Fact]
  public void Shortest_Route_ShouldAddNewEdge_Test() {
    // Arrange
    Shortest_Route_Class graph = new Shortest_Route_Class(3, 0);
    // Act
    graph.AddEdge(0, 1, 4);
    graph.AddEdge(1, 2, 3);
    // Assert
    Assert.Equal(2, graph.Edges.Count);
  }

  [Fact]
  public void Shortest_Route_ShouldAddNewEdge_Properties_Test() {
    // Arrange
    Shortest_Route_Class graph = new Shortest_Route_Class(3, 0);
    // Act
    graph.AddEdge(0, 1, 4);
    graph.AddEdge(1, 2, 3);
    Shortest_Route_Class.Edge edge = graph.Edges[1];
    // Assert
    Assert.Equal(1, edge.SourceVertex);
    Assert.Equal(2, edge.DestinationVertex);
    Assert.Equal(3, edge.Lenght);
  }
}

/// <summary>
/// The tests include compressing and decompressing text and MP3 files using the Huffman coding algorithm.
/// </summary>
public class File_Compression_Decompression_Test {

  [Fact]
  public void LoremIpsum_HuffmanCoding_Txt_Test() {
    string mp3_File_Path = "music.mp3";
    string txt_File_Path = "Input_Txt.txt";
    long fileSize = new FileInfo(mp3_File_Path).Length;
    string loremIpsum = File_Compression_Decompression.Huffman_Helper.CreatingLoremIpsum(fileSize);
    File.WriteAllText(txt_File_Path, loremIpsum);
    string txt_Input_File_Path = "Input_Txt.txt";
    string txt_Comp_File_Path = "Compressed_Txt.bin";
    string detxt_Comp_File_Path = "Output_Txt.txt";
    string input = File.ReadAllText(txt_Input_File_Path);
    File_Compression_Decompression.HuffmanTree_Class tree = new File_Compression_Decompression.HuffmanTree_Class();
    tree.Build(input);
    BitArray encoded = tree.Encode(input);
    using (FileStream compressedFileStream = new FileStream(txt_Comp_File_Path, FileMode.Create)) {
      using (BinaryWriter writer = new BinaryWriter(compressedFileStream)) {
        WriteBitArray(writer, encoded);
      }
    }
    using (FileStream compressedFileStream = new FileStream(txt_Comp_File_Path, FileMode.Open)) {
      using (BinaryReader reader = new BinaryReader(compressedFileStream)) {
        BitArray encodedFromFile = ReadBitArray(reader, encoded.Length);
        string decoded = tree.Decode(encodedFromFile);
        File.WriteAllText(detxt_Comp_File_Path, decoded);
      }
    }
    string decompressed = File.ReadAllText(detxt_Comp_File_Path);
    Assert.Equal(input, decompressed);
  }

  void WriteBitArray(BinaryWriter writer, BitArray bits) {
    byte[] bytes = new byte[(bits.Length + 7) / 8];
    bits.CopyTo(bytes, 0);
    writer.Write(bytes);
  }

  BitArray ReadBitArray(BinaryReader reader, int bitCount) {
    List<bool> bits = new List<bool>();
    byte[] bytes = reader.ReadBytes((bitCount + 7) / 8);

    foreach (byte b in bytes) {
      for (int i = 0; i < 8; i++) {
        bits.Add((b & (1 << i)) != 0);
      }
    }

    // Truncate any extra bits that may have been read in the last byte
    int extraBits = bits.Count - bitCount;

    if (extraBits > 0) {
      bits.RemoveRange(bitCount, extraBits);
    }

    return new BitArray(bits.ToArray());
  }

  [Fact]
  public void File_Compression_Decompression_Mp3_Test() {
    string txt_Input_File_Path = "music.mp3";
            string txt_Comp_File_Path = "music.bin";
    string detxt_Comp_File_Path = "music.mp3";
            // Read the file contents
            byte[] input = File.ReadAllBytes(txt_Input_File_Path);
    // Create Huffman tree and encode the input file
    File_Compression_Decompression.HuffmanTree_Class_Music tree = new File_Compression_Decompression.HuffmanTree_Class_Music();
    tree.Build(input);
    BitArray encoded = tree.Encode(input);
    // Write the encoded bits to binary file
    using (FileStream compressedFileStream = new FileStream(txt_Comp_File_Path, FileMode.Create)) {
      using (BinaryWriter writer = new BinaryWriter(compressedFileStream)) {
        File_Compression_Decompression.Huffman_Helper.WriteBitArray(writer, encoded);
      }
    }
    // Read the compressed file and the encoded bits
    using (FileStream compressedFileStream = new FileStream(txt_Comp_File_Path, FileMode.Open)) {
      using (BinaryReader reader = new BinaryReader(compressedFileStream)) {
        BitArray encodedFromFile = File_Compression_Decompression.Huffman_Helper.ReadBitArray(reader, compressedFileStream.Length);
        // Decode the encoded bits with Huffman tree
        byte[] decoded_Bytes = tree.Decode(encodedFromFile);
        // Write the decoded bytes as the original file
        File.WriteAllBytes(detxt_Comp_File_Path, decoded_Bytes);
      }
    }
    // Read the decompressed file and compare with the original file
    byte[] decompressed_Bytes = File.ReadAllBytes(detxt_Comp_File_Path);
    Assert.Equal(input, decompressed_Bytes);
  }
}

/// <summary>
/// In this test, a list of edges representing a pipeline network is generated.
/// The KruskalAlgo method is run on this edge list and the minimum cost is calculated.
/// Then the calculated minimum cost is compared with the expected cost.
/// </summary>
public class Pipeline_System_Test_Class
{
    [Fact]
    public void Pipeline_System_Test()
    {
        List<Pipeline_System.Edge> edges = new List<Pipeline_System.Edge> {
        new Pipeline_System.Edge { StartN = 0, EndN = 1, Way = 1 },
        new Pipeline_System.Edge { StartN = 0, EndN = 2, Way = 1 },
        new Pipeline_System.Edge { StartN = 1, EndN = 2, Way = 3 },
        new Pipeline_System.Edge { StartN = 1, EndN = 3, Way = 3 },
        new Pipeline_System.Edge { StartN = 2, EndN = 3, Way = 8 },
        new Pipeline_System.Edge { StartN = 2, EndN = 4, Way = 3 },
        new Pipeline_System.Edge { StartN = 3, EndN = 4, Way = 1 },
        new Pipeline_System.Edge { StartN = 3, EndN = 4, Way = 1 },
        new Pipeline_System.Edge { StartN = 4, EndN = 5, Way = 6 },
        new Pipeline_System.Edge { StartN = 4, EndN = 5, Way = 2 }
        };
        Pipeline_System kruskal = new Pipeline_System(edges);
        int minimumcost = kruskal.KruskalAlgo(edges);
        int distances = minimumcost;
        Assert.Equal(14, distances);
    }
}

/// <summary>
/// This test creates an assembly guide according to the dependencies of the parts using the Belman with DFS method.
/// </summary>
public class Furniture_Assembly_Guide_Test_Class
{
    [Fact]
    public void Furniture_Assembly_Guide_Test()
    {
        List<Furniture> Furnitures = new List<Furniture>()
        {
        new Furniture("Wardrobe partition", 1),
        new Furniture("Cabinet floor", 2),
        new Furniture("Shelf floor", 3),
        new Furniture("Cabinet side doors", 4),
        new Furniture("Bottom post for cabinet side doors", 5),
        new Furniture("Cabinet Top cover", 6),
        new Furniture("Cabinet legs", 7),
        new Furniture("Cabinet Back cover", 8),
        new Furniture("Wall Mount hinge", 9),
        new Furniture("Cabinet Door", 10),
        new Furniture("Wardrobe Shelf Long", 11),
        new Furniture("Wardrobe Shelf Short", 12),
        };
        Furnitures[2].Dependencies.Add(1);
        Furnitures[3].Dependencies.Add(2);
        Furnitures[3].Dependencies.Add(3);
        Furnitures[4].Dependencies.Add(4);
        Furnitures[5].Dependencies.Add(1);
        Furnitures[5].Dependencies.Add(4);
        Furnitures[6].Dependencies.Add(5);
        Furnitures[7].Dependencies.Add(1);
        Furnitures[7].Dependencies.Add(4);
        Furnitures[7].Dependencies.Add(6);
        Furnitures[8].Dependencies.Add(8);
        Furnitures[9].Dependencies.Add(4);
        Furnitures[10].Dependencies.Add(1);
        Furnitures[10].Dependencies.Add(4);
        Furnitures[11].Dependencies.Add(1);
        Furnitures[11].Dependencies.Add(3);
        Furniture_Assembly_Guide furnitureAssemblyGuide = new Furniture_Assembly_Guide();
        furnitureAssemblyGuide.AddFurniture(Furnitures);
        ArrayList assemblySteps = furnitureAssemblyGuide.GetAssemblySteps();
        List<string> expectedSteps = new List<string>()
        {
            "1. Attach Wardrobe partition",
            "2. Attach Cabinet floor",
            "3. Attach Shelf floor",
            "4. Attach Cabinet side doors",
            "5. Attach Bottom post for cabinet side doors",
            "6. Attach Cabinet Top cover",
            "7. Attach Cabinet legs",
            "8. Attach Cabinet Back cover",
            "9. Attach Wall Mount hinge",
            "10. Attach Cabinet Door",
            "11. Attach Wardrobe Shelf Long",
            "12. Attach Wardrobe Shelf Short"
        };
        ArrayList actualSteps = furnitureAssemblyGuide.GetAssemblySteps();
        Assert.Equal(expectedSteps, actualSteps.Cast<string>());
    }
}

}
