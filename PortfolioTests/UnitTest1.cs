using Portfolio.WebApi.DTO.EducationDtos;
using Portfolio.WebApi.Extensions;
using Portfolio.WebApi.Models;

namespace PortfolioTests;

[TestClass]
public class UnitTest1
{
  [TestMethod]
  public void TestMethod1()
  {
    var asd = new List<KeyValuePair<string, string>> {
      new KeyValuePair<string, string>("hola", "chau"),
      new KeyValuePair<string, string>("hola", "chau"),
      new KeyValuePair<string, string>("hola", "chau"),
      new KeyValuePair<string, string>("hola", "chau"),
    };
    var qwe = asd.FirstOrDefault(s => s.Key == "qwe");

    var educationSearcheable = new EducationSearcheable
    {
      School = "asd"
    };
    var educationSearcheable2 = new EducationSearcheable();

    bool tuvieja = educationSearcheable.GetType().GetProperties().Any(p => p.GetValue(educationSearcheable) != null);
    bool tuvieja2 = educationSearcheable2.GetType().GetProperties().Any(p => p.GetValue(educationSearcheable2) != null);

    //Assert.IsTrue(tuvieja);
    //Assert.IsFalse(tuvieja2);
    var target = new EducationPostDto
    {
      School = "asdasdasdasdasd"
    };
    var source = new
    {
      School = 123
    };

    var target2 = new EducationPutDto
    {
      School = "asdasdasdasdasd"
    };
    var source2 = new EducationPutDto
    {
      School = "qweqweqweqweqwe"
    };

    try
    {
      target.SetTo(source); // adding extension method on a foreign project
      // before adding its reference makes it not recognize the method.
      // the reference must be deleted and added again.
      target2.SetTo(source2);
    } catch (Exception)
    {
      Console.WriteLine("puito");
    }
    var ed = new EducationPutDto
    {
      Degree = "ksdjfnksjngkjsdfn",
      EducationImg = "./assets/logos/asdasd.co",
      School = "sldkfmldkgmlf",
      StartDate = "03/11"
    };
    Assert.AreNotEqual(source.School, target.School);
    Assert.AreEqual(source2.School, target2.School);
    Assert.IsFalse(source2.Validate(out var Tuvieja));
    Assert.IsTrue(ed.Validate(out var Tuvieja2));
    ed.School = "asd";
    Assert.IsFalse(ed.Validate(out var Tuvieja3));
    Assert.IsTrue(Tuvieja3.Count() == 1);
  }
}