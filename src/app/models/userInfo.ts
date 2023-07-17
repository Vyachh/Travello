import { Trip } from './trip';
export interface IUserInfo {
    id: string
    userName: string
    currentTripId: number
    tripList: Trip[]
}