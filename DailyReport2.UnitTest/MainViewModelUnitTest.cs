using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyReport2.Services;
using DailyReport2.ViewModels;
namespace DailyReport2.UnitTest
{
    [TestClass] public class MainViewModelUnitTest
    {
        private FakeDialog d; private FakeSettings s; private FakeWriter w; private MainViewModel Create() { d=new FakeDialog(); s=new FakeSettings(); w=new FakeWriter(); return new MainViewModel(d,new FakeConfirm(),s,w); }
        [TestMethod] public void Constructor_初期化_今日と定例作業と空行が設定される() { var vm=Create(); Assert.AreEqual(DateTime.Today,vm.Date); Assert.AreEqual(1,vm.RegularTasks.Count); Assert.AreEqual("朝礼／夕礼",vm.RegularTasks[0].Content); Assert.AreEqual(1,vm.SkillUpActivities.Count); Assert.AreEqual(1,vm.Deliverables.Count); }
        [TestMethod] public void Validate_名前が空白_エラー() { var vm=Create(); vm.Name=" "; Assert.IsFalse(string.IsNullOrEmpty(vm.Validate())); }
        [TestMethod] public void Validate_リンクが相対URL_エラー() { var vm=Create(); vm.Name="山田"; vm.Deliverables[0].Name="資料"; vm.Deliverables[0].Link="/file"; Assert.IsTrue(vm.Validate().Contains("http")); }
        [TestMethod] public void Validate_リンクがHttps絶対URL_成功() { var vm=Create(); vm.Name="山田"; vm.Deliverables[0].Name="資料"; vm.Deliverables[0].Link="https://example.com/a"; Assert.IsNull(vm.Validate()); }
        [TestMethod] public void Validate_両リストに入力なし_エラー() { var vm=Create(); vm.Name="山田"; vm.RegularTasks.Clear(); vm.SkillUpActivities.Clear(); Assert.IsTrue(vm.Validate().Contains("定例作業")); }
        [TestMethod] public void Validate_日付が未指定_エラー() { var vm=Create(); vm.Name="山田"; vm.Date=DateTime.MinValue; Assert.IsTrue(vm.Validate().Contains("日付")); }
        [TestMethod] public void Save_キャンセル_書込みされない() { var vm=Create(); vm.Name="山田"; d.Path=null; vm.Save(); Assert.IsFalse(w.Written); Assert.AreEqual(1,vm.RegularTasks.Count); }
        [TestMethod] public void Save_正常_マークダウンが書込まれる() { var vm=Create(); vm.Name="山田"; d.Path="report.md"; vm.Save(); Assert.IsTrue(w.Written); Assert.IsTrue(w.Content.Contains("# 日報")); Assert.AreEqual(1,vm.RegularTasks.Count); }
        [TestMethod] public void Save_既存を拒否_書込みされない() { var vm=Create(); vm.Name="山田"; d.Path="report.md"; w.Existing=true; vm.Save(); Assert.IsFalse(w.Written); Assert.AreEqual(1,vm.RegularTasks.Count); }
        [TestMethod] public void Save_書込み失敗_エラー表示と入力維持() { var vm=Create(); vm.Name="山田"; d.Path="report.md"; w.Failure=new InvalidOperationException("ディスクエラー"); vm.Save(); Assert.IsTrue(vm.ErrorMessage.Contains("保存に失敗")); Assert.AreEqual(1,vm.RegularTasks.Count); Assert.AreEqual("朝礼／夕礼",vm.RegularTasks[0].Content); }
        [TestMethod] public void Save_IOException_エラー表示と入力維持() { var vm=Create(); vm.Name="山田"; d.Path="report.md"; w.Failure=new IOException("入出力エラー"); vm.Save(); Assert.IsTrue(vm.ErrorMessage.Contains("保存に失敗")); Assert.AreEqual(1,vm.RegularTasks.Count); }
        private class FakeDialog:IFileDialogService { public string Path; public string ShowSaveDialog(string n){return Path;} }
        private class FakeSettings:ISettingsService { public string Name {get;set;} }
        private class FakeConfirm:IConfirmationService { public bool Confirm(string m,string t){return false;} }
        private class FakeWriter:IFileWriter { public bool Existing; public bool Written; public string Content; public Exception Failure; public bool Exists(string p){return Existing;} public void WriteAllText(string p,string c){if(Failure != null) throw Failure; Written=true;Content=c;} }
    }
}
