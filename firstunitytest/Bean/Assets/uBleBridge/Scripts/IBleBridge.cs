﻿using System;
using System.Collections.Generic;

namespace startechplus.ble
{
	/** 
	 * An Interface for connecting native Ble code to the rest of the applicaton.
	 */
	public interface IBleBridge 
	{

		/**
		 * This function is used to start the Bluetooth adapter. It initalizes the Bluetooth adapter and sets up the brige between native and Unity code.
		 * @param asCentral a bool indicating to start the bridge as Central or Peripheral mode
		 * @param action an Action callback when the native code is initalized
		 * @param errorAction an Action(string error) callback if there is an error on the native code
		 * @param stateUpdateAction an Action(string updateState) callback for Bluetooth adapter state updates. Valid values of updateState include: "Powered On", "Powered Off", "Resetting", "Unauthorized", "Unknown" and "Unsupported".
		 * @param rssiUpdateAction an Action(string peripherialId, string rssi) callback for Bluetooth device signal strength updates.
		 * @return a BluetoothLeDevice object for receiving native code callbacks, the BluetoothLeDevice is automatically generated by the approprate native bridge.
		 * @see BluetoothLeDevice
		 */
		BluetoothLeDevice Startup(bool asCentral, Action action, Action<string> errorAction, Action<string> stateUpdateAction, Action<string, string> rssiUpdateAction);

		/**
		 * This function is used to shutdown the Bluetooth adapter;
		 * @param action an Action callback when the Bluetooth adapter is shutdown
		 */
		void Shutdown(Action action);

		/**
		 * This function is used to release any resources allocated during Startup().  This should only be called if you don't plan on using the Bluetooth adapter again.
		 */ 
		void Cleanup();

		/**
		 * This function is used to put ble events in the background and stop reporting to Unity.
		 * @param isPaused a bool indicating wether to stop or continue ble activity callbacks.
		 */
		void PauseWithState(bool isPaused);

		/**
		 * This function is used to start scanning for ble devices.  It takes an array of service UUIDs and will only callback if the scan was successful and the device found has a service in the array.  If the array is null then all devices are returned.
		 * @param serviceUUIDs an array of services to filter found devices, or null to return all devices.
		 * @param action an Action(string peripherialId, string name) callback where peripheralId is the device identifier such as a MAC address, and name is the advertized human readable name not all devices support this.
		 */
		void ScanForPeripheralsWithServiceUUIDs(string[] serviceUUIDs, Action<string, string> action);

		/**
		 * Stops a already in progress scan.
		 */
		void StopScanning();

		/**
		 * This function is used to connect to a ble device to start communicating with it via its Characteristics and Descriptors.
		 * @param peripheralId a string used to identify the device you want to connect to. This is returned in the Action(string peripherialId, string name) callback from ScanForPeripheralsWithServiceUUIDs()
		 * @param connectAction an Action(string peripheralID, string name) callback for when the device has successfully connected
		 * @param serviceAction an Action(string peripherId, string service) callback for when a Service has been discovered
		 * @param characteristicAction an Action(string peripheralId, string service, string characteristic) callback for when a Characteristic has been discovered
		 * @param descriptorAction an Action(string peripheralId, string service, string characteristic, string descriptor) callback for when a Descriptor has beed discovered. 
		 * @param disconnectAction an Action(string peripheralId, string name) callback for when a device has been disconnected.
		 */ 
		void ConnectToPeripheralWithIdentifier(string peripheralId, Action<string, string> connectAction, Action<string, string> serviceAction, Action<string, string, string> characteristicAction, Action<string, string, string, string> descriptorAction, Action<string, string>disconnectAction);

		/**
		 * This function is used to disconnect to a ble device.
		 * @param peripheralId a string used to identify the device.
		 * @param action an Action(string peripheralId, string name) callback for when the device has successfuly been disconnected.
		 */ 
		void DisconnectFromPeripheralWithIdentifier(string peripheralId, Action<string, string> action);

		/**
		 * This funciton is similar to ScanForPeripheralsWithServiceUUIDs() except it doesn't look for new devices and is therefore faster.
		 * @param serviceUUIDs a string array used to filter the returned devices by it's advertized Services or null to return all devices.
		 * @param action an Action(string peripheralId, string name) callback for when the devices are found.
		 */ 
		void RetrieveListOfPeripheralsWithServiceUUIDs(string[] serviceUUIDs, Action<string, string> action);

		/**
		 * This function is similar to ScanForPeripheralsWithServiceUUIDs() except it doesn't look for ne devices and is therefore faster.
		 * @param uuids a string array used to filter the returned devices by there identifier, usually a MAC address.
		 * @param action an Action(string peripheralId, string name) callback for when the devices are found.
		 */ 
		void RetrieveListOfPeripheralsWithUUIDs(string[] uuids, Action<string, string> action);

		/**
		 * This function is used to get updates from Charactaristics as a Notification or Indication from the device.  
		 * The device itself sends the notification when it is available so there is no need to poll the device with ReadCharacteristicWithIdentifiers().
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param notificationAction an Action(string peripheralId, string service, string characteristic) callback for when the notification state was updated.
		 * @param action an Action(string peripheralId, string service, string characteristic, byte[] data) callback for when the Characteristic value has changed.
		 * @param isIndication a bool use to set the subscription as either a Notificaton or Indication, a Notification is not sent reliably and Indication is sent reliably and takes longer.
		 */ 
		void SubscribeToCharacteristicWithIdentifiers(string peripheralId, string serviceId, string characteristicId, Action<string, string, string> notificationAction, Action<string, string, string, byte[]> action, bool isIndication);

		/**
		 * This function is used to stop getting updates, either Notifications or Indications, from a Characteristic.
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param action an Action(string peripheralId, string service, string characteristic) callback for when the notification state was updated.
		 */ 
		void UnSubscribeFromCharacteristicWithIdentifiers(string peripheralId, string serviceId, string characteristicId, Action<string, string, string> action);

		/**
		 * This function is used to read the current value of a Characteristic.
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param action an Action(string peripheralId, string service, string characteristic, byte[] data) callback for when the Characteristic value has been read.
		 */ 
		void ReadCharacteristicWithIdentifiers(string peripheralId, string serviceId, string characteristicId, Action<string, string, string, byte[]> action);

		/**
		 * This function is used to write a value to a Characteristic. The maximum length of a ble packet / data is 20 bytes.
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param data a byte[] the value to write to the Characteristic
		 * @param length an int, the length of data
		 * @param withResponse a bool used to indicate you want a reliable (with response) or unreliable (without response) write, an unreliable write is substantally faster; withResponse = false
		 * @param action an Action(string peripheralId, string service, string characteristic) callback for when the Characteristic was successfully written to and withResponse = true;
		 */ 
		void WriteCharacteristicWithIdentifiers(string peripheralId, string serviceId, string characteristicId, byte[] data, int length, bool withResponse, Action<string, string, string> action);

		/**
		 * This funtion is used to read the value of a Descriptor.
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param descriptorId a string
		 * @param action an Action(string peripheralId, string service, string characteristic, string descriptor, byte[] data) callback for when the Descriptor value has been read.
		 */ 
		void ReadDescriptorWithIdentifiers(string peripheralId, string serviceId, string characteristicId, string descriptorId, Action<string, string, string, string, byte[]> action);

		/**
		 * This function is used to write a value to a Descriptor. The maximum length of a ble packet / data is 20 bytes.
		 * @param peripherialId a string
		 * @param serviceId a string
		 * @param characteristicId a string
		 * @param descriptorId a string
		 * @param data a byte[] the value to write to the Characteristic
		 * @param length an int, the length of data
		 * @param action an Action(string peripheralId, string service, string characteristic, string descriptor) callback for when the Descriptor was successfully written to.
		 */ 
		void WriteDescriptorWithIdentifiers(string peripheralId, string serviceId, string characteristicId, string descriptorId, byte[] data, int length, Action<string, string, string, string> action);

		/**
		 * This function is used to get a current RSSI Received Signal Strength Inicator value.
		 * @param peripherialId a string
		 */ 
		void ReadRssiWithIdentifier(string peripheralId);

	}
}
