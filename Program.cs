using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

static (Dictionary<string, string>, List<string>) FlattenJsonAndGenerateSettings(JToken jToken, string parentKey = "")
{
    var flattenedDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    var settings = new List<string>();

    if (jToken is JObject jObject)
    {
        foreach (var property in jObject.Properties())
        {
            var key = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";
            FlattenAndGenerateSettings(property.Value, key, flattenedDictionary, settings);
        }
    }
    else if (jToken is JArray jArray)
    {
        for (var i = 0; i < jArray.Count; i++)
        {
            var key = string.IsNullOrEmpty(parentKey) ? i.ToString() : $"{parentKey}[{i}]";
            FlattenAndGenerateSettings(jArray[i], key, flattenedDictionary, settings);
        }
    }
    else
    {
        flattenedDictionary[parentKey] = jToken.ToString();
        settings.Add(parentKey);
    }

    return (flattenedDictionary, settings);
}

static void FlattenAndGenerateSettings(JToken jToken, string currentKey, Dictionary<string, string> flattenedDictionary, List<string> settings)
{
    if (jToken is JObject jObject)
    {
        foreach (var property in jObject.Properties())
        {
            var key = $"{currentKey}.{property.Name}";
            FlattenAndGenerateSettings(property.Value, key, flattenedDictionary, settings);
        }
    }
    else if (jToken is JArray jArray)
    {
        for (var i = 0; i < jArray.Count; i++)
        {
            var key = $"{currentKey}[{i}]";
            FlattenAndGenerateSettings(jArray[i], key, flattenedDictionary, settings);
        }
    }
    else
    {
        flattenedDictionary[currentKey] = jToken.ToString();
        settings.Add(currentKey);
    }
}

var json = """
            {
                "apiName": "manageEndCust",
                "action": "addEndCust",
                "vipCode": "A604696",
                "isCallback": "true",
                "endCustomer": [
                    {
                        "endCustId": "CE6F37A4-56F1-4BEB-BA1F-516CA3D64D49",
                        "endCustAbb": "久年營造",
                        "endCustName": "久年營造股份有限公司",
                        "taxId": "12345678",
                        "country": "TW",
                        "postalCode": "00100",
                        "city": "台北市",
                        "province": "大安區",
                        "address1": "大安區忠孝東路3段6號2樓",
                        "address2": "#2345",
                        "contactLastName": "洪",
                        "contactFirstName": "金寶",
                        "contactEmail": "ABC@mail.com.tw",
                        "contactPhoneNo": "02-12345612",
                        "autodeskCSN": "5110220516",
                        "vdrAccount": [
                            {
                                "vdrCode": "Autodesk",
                                "endCustAcc": "ABC@mail.com.tw",
                                "vdrAgreements": {
                                    "lastName": "洪",
                                    "firstName": "金寶",
                                    "phone": "02-12345612",
                                    "email": "ABC@mail.com.tw",
                                    "dateAgreed": "2023-08-17"
                                }
                            },
                            {
                                "vdrCode": "Autodesk2",
                                "endCustAcc": "ABC2@mail.com.tw",
                                "vdrAgreements": {
                                    "lastName": "洪2",
                                    "firstName": "金寶",
                                    "phone": "02-12345612",
                                    "email": "ABC2@mail.com.tw",
                                    "dateAgreed": "2023-08-18"
                                }
                            }
                        ]
                    },
                    {
                        "endCustId": "CE6F37A4-56F1-4BEB-BA1F-516CA3D64D49",
                        "endCustAbb": "久年營造",
                        "endCustName": "久年營造股份有限公司",
                        "taxId": "12345678",
                        "country": "TW",
                        "postalCode": "00100",
                        "city": "台北市",
                        "province": "大安區",
                        "address1": "大安區忠孝東路3段6號2樓",
                        "address2": "#2345",
                        "contactLastName": "洪",
                        "contactFirstName": "金寶",
                        "contactEmail": "ABC@mail.com.tw",
                        "contactPhoneNo": "02-12345612",
                        "autodeskCSN": "5110220516",
                        "vdrAccount": [
                            {
                                "vdrCode": "Autodesk",
                                "endCustAcc": "ABC@mail.com.tw",
                                "vdrAgreements": {
                                    "lastName": "洪",
                                    "firstName": "金寶",
                                    "phone": "02-12345612",
                                    "email": "ABC@mail.com.tw",
                                    "dateAgreed": "2023-08-17"
                                }
                            },
                            {
                                "vdrCode": "Autodesk2",
                                "endCustAcc": "ABC2@mail.com.tw",
                                "vdrAgreements": {
                                    "lastName": "洪2",
                                    "firstName": "金寶",
                                    "phone": "02-12345612",
                                    "email": "ABC2@mail.com.tw",
                                    "dateAgreed": "2023-08-18"
                                }
                            }
                        ]
                    }
                ]
            }
    """;
Stopwatch stopwatch = new();
stopwatch.Reset();
stopwatch.Start();
var jToken = JToken.Parse(json);
var result = FlattenJsonAndGenerateSettings(jToken);
stopwatch.Stop();
Console.WriteLine($"同步執行耗時 {stopwatch.Elapsed.TotalMilliseconds} ms");

var flattenedDictionary = result.Item1;
var settings = result.Item2;

foreach (var kvp in flattenedDictionary)
{
    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
}

Console.WriteLine("Settings:");
foreach (var setting in settings)
{
    Console.WriteLine(setting);
}



static IDictionary<string, string> FlattenJsonToDictionary(JToken jToken, string parentKey = "")
{
    var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    switch (jToken)
    {
        case JObject jObject:
            {
                foreach (var property in jObject.Properties())
                {
                    var key = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";
                    var value = property.Value.ToString();

                    if (property.Value is JValue)
                    {
                        result[key] = value;
                    }
                    else
                    {
                        var nestedProperties = FlattenJsonToDictionary(property.Value, key);
                        foreach (var nestedProperty in nestedProperties)
                        {
                            result[nestedProperty.Key] = nestedProperty.Value;
                        }
                    }
                }

                break;
            }
        case JArray jArray:
            {
                for (var i = 0; i < jArray.Count; i++)
                {
                    var key = string.IsNullOrEmpty(parentKey) ? i.ToString() : $"{parentKey}[{i}]";
                    var value = jArray[i].ToString();

                    if (jArray[i] is JValue)
                    {
                        result[key] = value;
                    }
                    else
                    {
                        var nestedProperties = FlattenJsonToDictionary(jArray[i], key);
                        foreach (var nestedProperty in nestedProperties)
                        {
                            result[nestedProperty.Key] = nestedProperty.Value;
                        }
                    }
                }

                break;
            }
        case JValue jValue:
            result[parentKey] = jValue.ToString();
            break;
    }

    return result;
}

static IEnumerable<string> GenerateSettings(JToken jToken, string parentPath = "")
{
    var settings = new List<string>();

    if (jToken is JObject jObject)
    {
        foreach (var property in jObject.Properties())
        {
            var currentPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";
            settings.AddRange(GenerateSettings(property.Value, currentPath));
        }
    }
    else if (jToken is JArray jArray)
    {
        for (var i = 0; i < jArray.Count; i++)
        {
            var currentPath = string.IsNullOrEmpty(parentPath) ? i.ToString() : $"{parentPath}[{i}]";
            settings.AddRange(GenerateSettings(jArray[i], currentPath));
        }
    }
    else
    {
        settings.Add(parentPath);
    }

    return settings;
}

Stopwatch stopwatchA = new();
stopwatchA.Reset();
stopwatchA.Start();

var jTokenA = JToken.Parse(json);

var tasks = new List<Task>();

IDictionary<string, string> flattenedDictionaryA = null;
List<string> settingsA = null;

// 异步执行 FlattenJsonToDictionary 方法
tasks.Add(Task.Run(() =>
{
    flattenedDictionaryA = FlattenJsonToDictionary(jToken);
}));

// 异步执行 GenerateSettings 方法
tasks.Add(Task.Run(() =>
{
    settingsA = GenerateSettings(jToken).ToList();
}));

// 等待所有任务完成
await Task.WhenAll(tasks);
stopwatchA.Stop();
Console.WriteLine($"異步執行耗時 {stopwatchA.Elapsed.TotalMilliseconds} ms");

// 处理 FlattenJsonToDictionary 结果
foreach (var kvp in flattenedDictionaryA)
{
    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
}

// 处理 GenerateSettings 结果
Console.WriteLine("Settings:");
foreach (var setting in settingsA)
{
    Console.WriteLine(setting);
}