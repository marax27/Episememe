<template>
  <v-card dense>
    <Chart :options='options' />
  </v-card>
</template>

<script lang='ts'>
import { Component, Vue, Mixins } from 'vue-property-decorator';
import { Chart } from 'highcharts-vue';
import * as Highcharts from 'highcharts';
import { colors } from '../plugins/vuetify';
import { StatisticsDto } from './interfaces/StatisticsDto';
import ApiClientService from '../shared/mixins/api-client/api-client.service';

@Component({
  components: {
    'Chart': Chart,
  }
})
export default class StatisticsPanel extends Mixins(ApiClientService) {

  private textColor = '#cccccc';
  private darkTextColor = '#2a2a2a';
  private chartColor = colors.accent;

  mounted() {
    this.$api.get<StatisticsDto>('statistics')
      .then(response => {
        this.options.series[0].data = response.data.data;
      });
  }

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
      },
      min: 0
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
                [0, Highcharts.color(this.chartColor).get('rgba')],
                [1, Highcharts.color(this.chartColor).setOpacity(0).get('rgba')],
            ]
        },
        marker: {
          radius: 2
        },
        lineWidth: 1,
        lineColor: this.chartColor,
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
    loading: {
      hideDuration: 1000,
      showDuration: 1000,
    },
    series: [{
      type: 'area',
      name: 'No. of instances in repository',
      color: this.chartColor,
      data: [] as number[][]
    }]
  }
}
</script>
