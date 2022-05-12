import {EndpointUrlBuilder} from '@/api/helpers/endpointUrlBuilder'
import { INotification } from '@/models/INotification'
import api from './api'
import { getPageList } from './helpers/getPageListResponse'
import { IRequestParameters } from './request-features/IRequestParameters'

export async function fetchNotificationApi(parameter: IRequestParameters){
    const response = await api.get(EndpointUrlBuilder['notification'](parameter))
    return getPageList<INotification>(response)
}