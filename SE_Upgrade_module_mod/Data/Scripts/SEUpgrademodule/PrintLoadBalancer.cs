﻿using System.Collections.Generic;

namespace SEUpgrademodule
{
    public class PrintLoadBalancer
    {
        int m_periodLength = 0;

        int m_progress = 0;

        float m_progressPerTick = 0;

        int m_processedCount = 0;

        int m_initialSize = 0;

        Queue<UpgradeLogic> m_notProcessed = new Queue<UpgradeLogic>();

        public PrintLoadBalancer ()
        {
        }

        public void Update()
        {
            if(!(m_progress < m_periodLength))
            {
                m_periodLength = 60;

                m_progress = 0;

                m_notProcessed.Clear();

                foreach(var kv in Upgradecore.Upgrades)
                {
                    m_notProcessed.Enqueue(kv.Value);
                }

                m_initialSize = m_notProcessed.Count;

                m_processedCount = 0;

                m_progressPerTick = m_periodLength / ((float) m_notProcessed.Count);
            }

            m_progress += 1;

            int toProcess = ((int) (((float)m_progress/m_periodLength) * m_initialSize)) - m_processedCount;

            for(int i = 0; i < toProcess; i++)
            {
                if(m_notProcessed.Count > 0)
                {
                    var generator = m_notProcessed.Dequeue();

                    generator.UpdatePrintBalanced();
                }
            }

            m_processedCount += toProcess;
        }
    }
}

