using ABB.SrcML;
using ABB.SrcML.Data;
using ABB.Swum;
using ABB.Swum.Nodes;
using ABB.Swum.WordData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysisToolkit
{
    [TestFixture]
    public class SimpleSwumGenerator
    {
        private static ConservativeIdSplitter splitter;
        private static UnigramTagger tagger;
        private static PCKimmoPartOfSpeechData posData;

        [SetUp]
        public void initalizeSwum()
        {
            //initialize swum stuff
            splitter = new ConservativeIdSplitter();
            tagger = new UnigramTagger();
            posData = new PCKimmoPartOfSpeechData();
        }


        [TestCase]
        public void GenerateSimpleSwum()
        {
            var dataProject = new DataProject<CompleteWorkingSet>("npp_6.2.3",
                Path.GetFullPath("..//..//..//projects//npp_6.2.3"),
                "..//..//..//SrcML");

            dataProject.UpdateAsync().Wait();

            //get srcml stuff in order
            NamespaceDefinition globalNamespace;
            Assert.That(dataProject.WorkingSet.TryObtainReadLock(5000, out globalNamespace));

            //find an example method
            var guiMethod = globalNamespace.GetDescendants<MethodDefinition>().Where(m => m.Name == "saveGUIParams").First();
            var guiMethodXElement = DataHelpers.GetElement(dataProject.SourceArchive, guiMethod.PrimaryLocation);

            //generate swum for method declaration
            MethodContext mc = ContextBuilder.BuildMethodContext(guiMethodXElement);
            MethodDeclarationNode mdn = new MethodDeclarationNode("saveGUIParams", mc);
            BaseVerbRule rule = new BaseVerbRule(posData, tagger, splitter);
            Console.WriteLine("InClass = " + rule.InClass(mdn));
            rule.ConstructSwum(mdn);
            Console.WriteLine(mdn.ToString());
        }

        [TestCase]
        public void GenerateSwumForAnyOccassion()
        {
            var dataProject = new DataProject<CompleteWorkingSet>("Swum.NET-master",
                    Path.GetFullPath("..//..//..//projects//Swum.NET-master"),
                    "..//..//..//SrcML");

            dataProject.UpdateAsync().Wait();

            //get srcml stuff in order
            NamespaceDefinition globalNamespace;
            Assert.That(dataProject.WorkingSet.TryObtainReadLock(5000, out globalNamespace));

            //find an example method
            var sampleMethod = globalNamespace.GetDescendants<MethodDefinition>().Where(m => m.Name == "TestConstructSwum").First();
           
            foreach(var line in sampleMethod.GetDescendants())
            {
                var sampleMethod_MethodCalls = line.FindExpressions<MethodCall>();

                foreach (var methodCall in sampleMethod_MethodCalls)
                {
                    var swummedMdn = FromMethodCallToSWUM(methodCall, globalNamespace, dataProject);
                    if (swummedMdn != null)
                    {
                        Console.WriteLine("VOID RETURN");
                        Console.WriteLine(swummedMdn.ToString());
                    }
                }

                //return statement
                if (line is ReturnStatement)
                {
                    var returnMCall = line.FindExpressions<MethodCall>().First();
                    var swummedMdn = FromMethodCallToSWUM(returnMCall, globalNamespace, dataProject);
                    if(swummedMdn != null)
                    {
                        Console.WriteLine("RETURN STMT");
                        Console.WriteLine(swummedMdn.ToString());
                    }
                }
            }

        }


        private MethodDeclarationNode FromMethodCallToSWUM(MethodCall mcall, NamespaceDefinition globalNamespace, DataProject<CompleteWorkingSet> dataProject)
        {
            var mdef = globalNamespace.GetDescendants<MethodDefinition>().Where(decl => mcall.SignatureMatches(decl));
            if (mdef.Count() > 0)
            {
                var mdefXml = DataHelpers.GetElement(dataProject.SourceArchive, mdef.First().PrimaryLocation);
                MethodContext mc = ContextBuilder.BuildMethodContext(mdefXml);
                MethodDeclarationNode mdn = new MethodDeclarationNode(mcall.Name, mc);
                BaseVerbRule rule = new BaseVerbRule(posData, tagger, splitter);
                if (rule.InClass(mdn))
                {
                    rule.ConstructSwum(mdn);
                    return mdn;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Console.WriteLine(mcall.Name + " could not be found");
                return null;
            }
        }
    }
}
