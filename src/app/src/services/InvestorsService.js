import axios from 'axios'

const apiClient = axios.create({
  baseURL: `https://localhost:5001`,
  withCredentials: false, // This is the default
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  },
  timeout: 10000
})

export default {
  getDashboard(id) {
    return apiClient.get(`/investors/dashboard?investorId=${id}`)
  },
  updateScreeningCriteria(data){
    return apiClient.post(`/investors/`, data)
  }
  
}