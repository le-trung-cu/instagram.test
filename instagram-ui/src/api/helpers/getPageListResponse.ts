import { AxiosResponse, AxiosResponseHeaders } from "axios";
export type MetaData = {
  currentPage: number,
  totalPages: number,
  pageSize: number,
  totalCount: number,
  hasPrevious: boolean,
  hasNext: boolean,
}
export function getXPaginationHeader(response: AxiosResponseHeaders): MetaData{
  return JSON.parse(response['x-pagination']);
}

export function getPageList<T>(response: AxiosResponse): {pagination: MetaData,items: Array<T>}{
  return {
    pagination: JSON.parse(response.headers['x-pagination']),
    items: response.data,
  }
}