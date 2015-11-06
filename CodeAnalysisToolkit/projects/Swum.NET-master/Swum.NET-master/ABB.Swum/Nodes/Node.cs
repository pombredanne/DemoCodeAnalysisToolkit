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

namespace ABB.Swum.Nodes {
    /// <summary>
    /// An abstract node within the SWUM model
    /// </summary>
    public abstract class Node {
        private Location location;
        /// <summary>
        /// The location within the program.
        /// </summary>
        public virtual Location Location {
            get { return location; }
            protected set { location = value; }
        }

        /// <summary>
        /// Returns a string representation of the node without any SWUM markup.
        /// </summary>
        public virtual string ToPlainString() {
            return string.Empty;
        }

        /// <summary>
        /// Returns a PhraseNode containing the node's parsed name.
        /// </summary>
        public virtual PhraseNode GetParse() {
            return new PhraseNode();
        }
    }
}
