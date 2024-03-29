﻿/******************************************************************************
 * Copyright (c) 2012 ABB Group
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the Eclipse Public License v1.0
 * which accompanies this distribution, and is available at
 * http://www.eclipse.org/legal/epl-v10.html
 *
 * Contributors:
 *    Patrick Francis (ABB Group) - C# implementation and documentation
 *    Emily Hill (Univ. of Delaware) - Original design and implementation
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABB.Swum.Nodes;
using ABB.Swum.WordData;

namespace ABB.Swum
{
    /// <summary>
    /// This rule assumes that it is the last rule being applied, and is therefore the default rule for methods.
    /// </summary>
    public class DefaultBaseVerbRule : BaseVerbRule
    {
        /// <summary>
        /// Creates a new DefaultBaseVerbRule using default values for data sets.
        /// </summary>
        public DefaultBaseVerbRule() : base() { }

        /// <summary>
        /// Creates a new DefaultBaseVerbRule.
        /// </summary>
        /// <param name="posData">The part-of-speech data to use.</param>
        /// <param name="tagger">The part-of-speech tagger to use.</param>
        /// <param name="splitter">The identifier splitter to use.</param>
        public DefaultBaseVerbRule(PartOfSpeechData posData, Tagger tagger, IdSplitter splitter) : base(posData, tagger, splitter) { }

        /// <summary>
        /// Creates a new DefaultBaseVerbRule.
        /// </summary>
        /// <param name="specialWords">A list of words that indicate the method name needs special handling.</param>
        /// <param name="booleanArgumentVerbs">A list of verbs that indicate that the boolean arguments to a method should be included in the UnknownArguments list.</param>
        /// <param name="nounPhraseIndicators">A list of word that indicate that beginning of a noun phrase.</param>
        /// <param name="positionalFrequencies">Positional frequency data.</param>
        public DefaultBaseVerbRule(HashSet<string> specialWords, HashSet<string> booleanArgumentVerbs, HashSet<string> nounPhraseIndicators, PositionalFrequencies positionalFrequencies)
            : base(specialWords, booleanArgumentVerbs, nounPhraseIndicators, positionalFrequencies) { }

        /// <summary>
        /// Creates a new DefaultBaseVerbRule.
        /// </summary>
        /// <param name="posData">The part-of-speech data to use.</param>
        /// <param name="tagger">The part-of-speech tagger to use.</param>
        /// <param name="splitter">The identifier splitter to use.</param>
        /// <param name="specialWords">A list of words that indicate the method name needs special handling.</param>
        /// <param name="booleanArgumentVerbs">A list of verbs that indicate that the boolean arguments to a method should be included in the UnknownArguments list.</param>
        /// <param name="nounPhraseIndicators">A list of word that indicate that beginning of a noun phrase.</param>
        /// <param name="positionalFrequencies">Positional frequency data.</param>
        public DefaultBaseVerbRule(PartOfSpeechData posData, Tagger tagger, IdSplitter splitter, HashSet<string> specialWords, HashSet<string> booleanArgumentVerbs, HashSet<string> nounPhraseIndicators, PositionalFrequencies positionalFrequencies)
            : base(posData, tagger, splitter, specialWords, booleanArgumentVerbs, nounPhraseIndicators, positionalFrequencies) { }

        /// <summary>
        /// Determines whether the given node meets the conditions of this rule.
        /// Since this is a default rule, it returns True for any MethodDeclarationNode.
        /// </summary>
        /// <param name="node">The node to test.</param>
        /// <returns>True if the given node is a MethodDeclarationNode, False otherwise.</returns>
        public override bool InClass(ProgramElementNode node)
        {
            return (node is MethodDeclarationNode);
        }

        /// <summary>
        /// Constructs the SWUM for the given node, using this rule.
        /// </summary>
        /// <param name="node">The node to construct SWUM for.</param>
        public override void ConstructSwum(ProgramElementNode node)
        {
            if (node is MethodDeclarationNode)
            {
                ((MethodDeclarationNode)node).Parse(this.Splitter);
                this.PosTagger.PreTag(node);
                base.ConstructSwum(node);
            }
        }
    }
}
