import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';

export default class ApiClient {
  constructor(
    private _auth: any | null,
    private _apiUrl: string) {}

  public async get<T>(resource: string, extraHeaders: {[key: string]: string} = {}): Promise<AxiosResponse<T>> {
    const config = await this.getConfig(extraHeaders);
    const url = this.getUrl(resource);
    return axios.get<T>(url, config);
  }

  public async delete<T>(resource: string, extraHeaders: {[key: string]: string} = {}): Promise<AxiosResponse<T>> {
    const config = await this.getConfig(extraHeaders);
    const url = this.getUrl(resource);
    return axios.delete<T>(url, config);
  }

  public async post<T>(resource: string, data: any, extraHeaders: {[key: string]: string} = {}): Promise<AxiosResponse<T>> {
    const config = await this.getConfig(extraHeaders);
    const url = this.getUrl(resource);
    return axios.post<T>(url, data, config);
  }

  public async put<T>(resource: string, data: any, extraHeaders: {[key: string]: string} = {}): Promise<AxiosResponse<T>> {
    const config = await this.getConfig(extraHeaders);
    const url = this.getUrl(resource);
    return axios.put<T>(url, data, config);
  }

  public async patch<T>(resource: string, data: any, extraHeaders: {[key: string]: string} = {}): Promise<AxiosResponse<T>> {
    const config = await this.getConfig(extraHeaders);
    const url = this.getUrl(resource);
    return axios.patch<T>(url, data, config);
  }

  public createUrl(resource: string, queryParameters: {[key: string]: string}): string {
    const baseUrl = this.getUrl(resource);
    const params = Object.keys(queryParameters)
      .map(key => `${key}=${encodeURIComponent(queryParameters[key])}`)
      .join('&');
    return baseUrl + (params ? '?' + params : '');
  }

  private getUrl(apiResource: string): string {
    return `${this._apiUrl}/${apiResource}`;
  }

  private async getConfig(extraHeaders: {[key: string]: string}): Promise<AxiosRequestConfig> {
    const baseHeaders: {[key: string]: string} = {};
    if (this._auth != null) {
      const token = await this._auth.getTokenSilently();
      baseHeaders['Authorization'] = `Bearer ${token}`;
    }

    return {
      headers: {...baseHeaders, ...extraHeaders},
      timeout: 5000
    };
  }
}
