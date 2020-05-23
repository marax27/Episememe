<template>
  <v-card dense>
    <Chart :options='options' />
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue } from 'vue-property-decorator';
import { Chart } from 'highcharts-vue';
import * as Highcharts from 'highcharts';
import { colors } from '../plugins/vuetify';
import { mockData } from './mockData';

@Component({
  components: {
    'Chart': Chart,
  }
})
export default class StatisticsPanel extends Vue {

  private textColor = '#cccccc';
  private darkTextColor = '#2a2a2a';

  options = {
    chart: {
      zoomType: 'x',
      style: {
        fontFamily: 'Titillium Web'
      },
      backgroundColor: 'transparent',
      resetZoomButton: {
        position: {
          align: "left",
          verticalAlign: "top"
        }
      }
    },
    title: {
      text: 'Media instance count over time',
      style: {
        color: this.textColor
      }
    },
    subtitle: {
      text: document.ontouchstart === undefined
        ? 'Click and drag in the plot area to zoom in'
        : 'Pinch the chart to zoom in',
      style: {
        color: this.textColor
      }
    },
    xAxis: {
      type: 'datetime',
      labels: {
        formatter: function(): string {
          return Highcharts.dateFormat('%Y-%m-%d', (this as any).value);
        },
        style: {
          color: this.textColor
        }
      }
    },
    yAxis: {
      title: {
        text: 'Media count',
        style: {
          color: this.textColor
        }
      },
      labels: {
        style: {
          color: this.textColor
        }
      }
    },
    legend: {
      enabled: false
    },
    plotOptions: {
      area: {
        fillColor: {
            linearGradient: {
                x1: 0,
                y1: 0,
                x2: 0,
                y2: 1
            },
            stops: [
                [0, Highcharts.color(colors.accent).get('rgba')],
                [1, Highcharts.color(colors.accent).setOpacity(0).get('rgba')],
            ]
        },
        marker: {
          radius: 2
        },
        lineWidth: 1,
        lineColor: colors.accent,
        states: {
          hover: {
            lineWidth: 1
          }
        },
        threshold: null
      }
    },
    tooltip: {
      backgroundColor: '#cecece',
      xDateFormat: '%Y-%m-%d',
      style: {
        color: this.darkTextColor
      }
    },
    series: [{
      type: 'area',
      name: 'No. of instances in repository',
      color: colors.accent,
      data: mockData
    }]
  }
}
</script>
