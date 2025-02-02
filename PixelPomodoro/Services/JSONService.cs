using System.Text.Json;

namespace PixelPomodoro.Services;

public class JSONService {
    public static T GetJSONData<T>(string filePath) {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
    public static void SaveJSONData<T>(T data, string filePath) {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
}