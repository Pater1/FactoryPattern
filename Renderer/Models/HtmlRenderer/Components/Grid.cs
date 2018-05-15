﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Factory.Components {
    public class Grid: Component {
        private int width;
        public int Width {
            get {
                return width;
            }

            set {
                width = value > 1 ? value : 1;
            }
        }
        private int height;
        public int Height {
            get {
                return height;
            }

            set {
                height = value > 1 ? value : 1;
            }
        }

        public override ChildrenHandling ChildrenSupported => ChildrenHandling.multiple;

        public override void Add(Component child) {
            //check component fits within grid
            if(child.X < Width && child.X + child.XSpan < Width &&
                child.Y < Height && Child.Y + child.YSpan < Height) {
                //check no components overlap this one
                Func<(int, int), Component, bool> overlap = (p, c) => {
                    return (p.Item1 >= c.X && p.Item1 <= c.X + c.XSpan) && (p.Item2 >= c.Y && p.Item2 <= c.Y + c.YSpan);
                };
                Func<Component, Component, bool> collision = (c1, c2) => {
                    (int, int) tlc = (c1.X, c1.Y);
                    (int, int) trc = (c1.X + c1.XSpan, c1.Y);
                    (int, int) blc = (c1.X, c1.Y + c1.YSpan);
                    (int, int) brc = (c1.X + c1.XSpan, c1.Y + c1.YSpan);


                    return overlap(tlc, c2) || overlap(trc, c2) || overlap(blc, c2) || overlap(brc, c2);
                };
                foreach(Component c in this){
                    if(collision(child,c) || collision(c, child)){
                        return;
                    }
                }
                
                //checks out, add to child list
                base.Add(child);
            }
        }
    }
}