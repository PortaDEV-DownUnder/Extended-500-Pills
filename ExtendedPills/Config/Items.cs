using System.Collections.Generic;
using ExtendedPills.Items;

namespace ExtendedPills.Config
{
    public class Items
    {
        public List<SCP_500_A> A { get; set; } = new() { new SCP_500_A() };
        public List<SCP_500_B> B { get; set; } = new() { new SCP_500_B() };
        public List<SCP_500_D> D { get; set; } = new() { new SCP_500_D() };
        public List<SCP_500_H> H { get; set; } = new() { new SCP_500_H() };
        public List<SCP_500_N> N { get; set; } = new() { new SCP_500_N() };
        public List<SCP_500_S> S { get; set; } = new() { new SCP_500_S() };
        public List<SCP_500_T> T { get; set; } = new() { new SCP_500_T() };
        
    }
}